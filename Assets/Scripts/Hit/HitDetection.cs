using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 공격 판정
public enum ENABLE_TYPE 
{
    Always,         // 비활성화 되지 않음
    Projectile,     // 벽또는 시간이 지나면 비활성화 됨
    Time,           // 시간이 지나면 비활성화 됨
    NONE 
}

public class HitDetection : MonoBehaviour
{
    [field: SerializeField] public int AttackID;                     // 중복 타격 방지용 아이디
    [field: SerializeField] public ObjectBasic user {get; set; }
    
    [field: SerializeField] float DefaultDamage;                  // 기본 피해량                       
    [field: SerializeField] public float knockBack { get; protected set; }                     // 넉백 수치
    [field: SerializeField] public List<int> statusEffect { get; protected set; }


    // 아래에 대상은 참조 형태로만 넘겨주어야지 활성화
    public Stat critical { get; protected set; }                                                // 치명타 확률
    public Stat criticalDamage { get; protected set; }                                          // 치명타 피해량
    public Stat DamageType { get; protected set; }                                              // 계수에 영향 받을 타입
    float Ratio;                                                 // 공격 계수

    #region Module
    [field: Header("Hit Module")]

    [Tooltip("활성화 후 지난 시간")]
    [field: SerializeField] float EnableTime;

    [field: Header("Pool")]
    [Tooltip("true 시 파괴되는 대신 비활성화 됩니다.(Defult false)")]
    [field: SerializeField] bool isPooling;                                // 풀링 시스템 여부

    [field: Header("Attack ID Managing")]
    [Tooltip("true 시 HD가 직접 AttackID를 관리합니다.(Defult true)")]
    [field: SerializeField] bool isAttackIDManage = true;                                // 풀링 시스템 여부


    [field: Header("Multi Hit")]
    [Tooltip("true 시 다단 히트가 가능해집니다.(Defult false)")]
    [field: SerializeField] bool isMultiHit;                                // 다단히트 여부
    [field: SerializeField] float DPS = 1;                                      // 초당 타격 횟수 필요 없을 시 음수
    Collider2D hitCollider;                                                 // 콜라이더 OnOff 용
    float multiHitDurationTime = 0;                                         // 간격 타이버

    EnemyStats enemyStats;

    [field: Header("Enable Type")]
    [Tooltip("Always : 비활성화 X, Projectile : 충돌 시 또는 시간에 따라 비활성화, Time : 시간에 따라")]
    [field: SerializeField] ENABLE_TYPE EnableType;                         // 활성화 타입
    [field: SerializeField] int penetrations;                               // 관통 횟수 필요 없을 시 음수
    [field: SerializeField] protected float DisableTime = 0.0f;             // Projectile, Time 시 해당 시간 이후에 비활화됨

    
    [Serializable]
    public class DisableObject
    {
        [SerializeField] bool IsPooling;
        [SerializeField] GameObject Object;
        [Range(0.0f, 1.0f), SerializeField] float Range = 1.0f;

        public DisableObject(bool _IsPool, GameObject _DisableObject, float _Range = 1.0f)
        {
            IsPooling = _IsPool;
            Object = _DisableObject;
            Range = Mathf.Clamp(_Range,0.0f,1.0f);
        }

        public bool GetIsPooling() {return IsPooling;}
        public GameObject GetObject() { return Object; }
        public float GetRange() { return Range; }
    }
    [Header("Disable Object")]
    [Tooltip("오브젝트가 비활성화되면 추가 오브젝트를 생성합니다. (Defult false)")]
    [field: SerializeField] List<DisableObject> DisableObjects;

    [field: Header("Guide")]
    [Tooltip("true 시 GuideTime 동안 GuideEffect가 활성화 되며 GuideTime 후에 GuideEndEffect가 활성화 되고 본격적으로 판정이 시작됩니다. (Defult false)")]
    [field: SerializeField] bool isGuide;
    [field: SerializeField] float GuideTime;
    [Tooltip("가이드 중일 때 보여주는 가이드")]
    [field: SerializeField] GameObject GuideEffect;
    [Tooltip("가이드가 끝났을 시 나타나는 이펙트")]
    [field: SerializeField] GameObject GuideEndEffect;
    [field: SerializeField] bool FinishGuide;

    [Header("Growing")]
    [Tooltip("오브젝트가 활성화되고 계속 크기 커집니다. (Default false)")]
    [field: SerializeField] bool isGrowing;
    [field: SerializeField] Vector3 DefaultScale;

    [Tooltip("초당 GPS만큼 크기가 증가합니다.")]
    [field: SerializeField] float GPS;

    #region 미완성
    [field: Header("미완성")]
    [field: Header("Hit Succes Effect Instant")]
    [Tooltip("true 시 타격 성공 시 추가 오브젝트를 생성합니다. (Instant) (Defult false)")]
    [field: SerializeField] bool isHitSuccesEffect_Instant;
    [field: SerializeField] GameObject HitSuccesEffectObjcet_Instant;

    [field: Header("Hit Succes Effect Pool")]
    [Tooltip("true 시 타격 성공 시 추가 오브젝트를 생성합니다. (Pool) (Defult false)")]
    [field: SerializeField] bool isHitSuccesEffect_Pool;
    [field: SerializeField] string HitSuccesEffectObjcet_Pool;

    [field: Header("Guided")]
    [Tooltip("true 시 타겟을 추적합니다. (Defult false)")]
    [field: SerializeField] bool isGuided;
    [field: SerializeField] Transform GuidingTarget;
    [field: SerializeField] int GuidingSpeed;
    [field: SerializeField] int GuidingAngularSpeed;

    [field: Header("Rotate")]
    [Tooltip("true 시 회전합니다. (Defult false)")]
    [field: SerializeField] bool isRotate;
    [field: SerializeField] float RoatateAngulerSpeed;
    #endregion 미완성


    #endregion Module

    void Awake()
    {
        hitCollider = GetComponent<Collider2D>();
        enemyStats=GetComponent<EnemyStats>();
    }

    void Update()
    {
        EnableTime += Time.deltaTime;

        // 가이드 중일 때는 아래가 활성화되지 않음
        if (isGuide)
        {
            if (!FinishGuide)
                return;
        }

        // 활성화 시 적정 텀마다 콜라이더랄 OnOff 함
        if (isMultiHit)
        {
            multiHitDurationTime += Time.deltaTime;
            if (multiHitDurationTime > (1f / DPS))
            {
                StartCoroutine(ColliderOnOff());
                multiHitDurationTime -= (1f / DPS);
            }
        }

        // 해당 타입일 시 시간이 지나면 비활성화 됨
        if (EnableType == ENABLE_TYPE.Projectile || EnableType == ENABLE_TYPE.Time)
        {
            DisableTime -= Time.deltaTime;
            if (DisableTime <= 0)
            {
                DisableHD();
            }
        }

        // isGrowing 활성화 시 크기가 커진다.
        if (isGrowing)
        {
            transform.localScale = DefaultScale * (1 + EnableTime * GPS) ;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (EnableType == ENABLE_TYPE.Projectile)
        {
            if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Door" || other.gameObject.tag == "ShabbyWall"
                 || other.CompareTag("EnemyWall"))
            {
                DisableHD();
            }

            if (!this.isMultiHit)
            {
                if (this.penetrations >= 1)
                {
                    penetrations--;
                }
                else if (this.penetrations == 0)
                {
                    DisableHD();
                }
            }
        }

        if (!user)
            return;

        if (!other.gameObject.GetComponent<ObjectBasic>())
            return;

        if (other.gameObject.GetComponent<ObjectBasic>().status.isInvincible)
            return;

        user.GetComponent<ObjectBasic>().status.hitTarget = other.gameObject;
    }

    void OnEnable()
    {
        if (isAttackIDManage)
            AttackID = Guid.NewGuid().GetHashCode();

        if (isGuide)
            StartCoroutine("Guide");

        if (EnableType == ENABLE_TYPE.Projectile || EnableType == ENABLE_TYPE.Time)
        {
            // 비활성화 시간 기본값으로 리셋
            if (enemyStats == null) { DisableTime = 10; }
            else { DisableTime = enemyStats.disableTime; }
        }

        if (isGrowing)
        {
            DefaultScale = transform.localScale;
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    #region Set

    public void SetAttackID(int _AttackID)
    {
        AttackID = _AttackID;
    }

    // 공격력에 영향을 받지않는 공격
    public void SetDamage(float DefaultDamage, float knockBack = 0.0f)
    {
        this.DefaultDamage = DefaultDamage;
        this.knockBack = knockBack;
    }


    // 해당 공격 유형 비율에 따라 피해량이 변하는 공격 설정
    public void SetHit_Ratio(
    float DefaultDamage, float Ratio, Stat DamageType, float knockBack = 0.0f, Stat critical = null, Stat criticalDamage = null)
    {
        this.DefaultDamage = DefaultDamage;
        this.knockBack = knockBack;
        this.critical = critical;
        this.criticalDamage = criticalDamage;

        this.Ratio = Ratio;
        this.DamageType = DamageType;


    }

    // 투사체 설정
    // 해당 공격 유형 비율에 따라 피해량이 변하는 공격 설정
    public void SetProjectile_Ratio(int penetrations, 
    float DefaultDamage, float Ratio, Stat DamageType, float knockBack = 0.0f, Stat critical = null, Stat criticalDamage = null)
    {
        this.DefaultDamage = DefaultDamage;
        this.knockBack = knockBack;
        this.critical = critical;
        this.criticalDamage = criticalDamage;

        this.Ratio = Ratio;
        this.DamageType = DamageType;

        this.EnableType = ENABLE_TYPE.Projectile;
        this.penetrations = penetrations;


    }

    // 다단히트 설정
    public void SetMultiHit(bool isMultiHit, int DPS)
    {
        this.isMultiHit =isMultiHit;
        this.DPS = DPS;
    }

    // 디버프 설정 하나만
    public void SetSE(int statusEffect)
    {
        this.statusEffect.Clear();
        if(statusEffect < 0 || GameData.instance.statusEffectList.Count < statusEffect)
            return;
        this.statusEffect.Add(statusEffect);
    }

    // 디버프 설정 여러개
    public void SetSEs(int[] statusEffect)
    {
        // 디버프 적용
        this.statusEffect.Clear();
        foreach (int i in statusEffect)
        {
            if (i < 0 || GameData.instance.statusEffectList.Count < i)
                return;
            this.statusEffect.Add(i);
        }
    }

    /// <summary>
    /// 비활성화 시간
    /// </summary>
    /// <param name="time"> Enabla_Type이 Projectile, Time 이라면 해당 시간 후에 비활성화 된다.</param>
    /// <param name="_Type"> Enable_Type을 변경한다</param>
    public void SetDisableTime(float time, ENABLE_TYPE _Type = ENABLE_TYPE.NONE)
    {
        EnableType = _Type == ENABLE_TYPE.NONE ? EnableType : _Type;
        DisableTime = time;
    }

    /// <summary>
    /// 비활성화 시 추가 생성되는 오브젝트
    /// </summary>
    /// <param name="_DisableObject"></param>
    public void SetDisableObject(bool _IsPool, GameObject _DisableObject, float _Range = 1.0f)
    {
        DisableObjects.Add(new DisableObject(_IsPool, _DisableObject, _Range));

    }

    public void SetGrowing(bool _IsGrowing, float _GPS)
    {
        isGrowing = true;
        GPS = _GPS;
        DefaultScale = transform.localScale;
    }

    #endregion Set

    // 비활성화 한다.
    void DisableHD()
    {

        for(int i = 0; i <DisableObjects.Count; ++i)
        {


            if (DisableObjects[i].GetIsPooling())
            {
                int prob = UnityEngine.Random.Range(0, 100);
                if (prob < DisableObjects[i].GetRange() * 100)
                {
                    GameObject PoolObject = ObjectPoolManager.instance.Get(DisableObjects[i].GetObject());
                    PoolObject.transform.position = this.gameObject.transform.position;
                    PoolObject.transform.rotation = this.gameObject.transform.rotation;
                }

            }
            else
            {
                int prob = UnityEngine.Random.Range(0, 100);
                if (prob < DisableObjects[i].GetRange() * 100)
                    Instantiate(DisableObjects[i].GetObject(), this.transform.position, Quaternion.identity);
            }
        }



        if (isPooling)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

    // 최종 피해량
    public float Damage
    {
        get
        {
            if (DamageType != null)
                return DefaultDamage + Ratio * DamageType.Value;
            return DefaultDamage;

        }
    }

    // 타격 판정 1 프레임 후 초기화
    public IEnumerator ColliderOnOff()
    {
        hitCollider.enabled = false;
        yield return null;
        hitCollider.enabled = true;
        if (isAttackIDManage)
            AttackID = Guid.NewGuid().GetHashCode();
    }

    // 가이드 
    IEnumerator Guide()
    {
        FinishGuide = false;

        GuideEffect.SetActive(true);
        GuideEndEffect.SetActive(false);
        hitCollider.enabled = false;

        yield return new WaitForSeconds(GuideTime);

        hitCollider.enabled = true;
        GuideEffect.SetActive(false);
        GuideEndEffect.SetActive(true);

        FinishGuide = true;
    }

}
