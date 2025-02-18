using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 공격 판정
public class HitDetection : MonoBehaviour
{
    
    [field: SerializeField] public int AttackID { get; protected set; }                      // 중복 타격 방지용 아이디
    [field: SerializeField] public ObjectBasic user;
    
    [field: SerializeField] public bool isProjectile { get; protected set; }                    // 투사체 여부
    [field: SerializeField] public int penetrations { get; set; }                               // 관통 횟수 필요 없을 시 음수
    [field: SerializeField] public bool isMultiHit { get; protected set; }                      // 다단히트 여부
    [field: SerializeField] public float DPS { get; protected set; }                              // 초당 타격 횟수 필요 없을 시 음수

    [field: SerializeField] public float DefaultDamage { get; protected set; }                  // 기본 피해량                       
    [field: SerializeField] public float Ratio { get; protected set; }                          // 공격 계수
    public Stat DamageType { get; protected set; }                      // 계수에 영향 받을 타입        


    [field: SerializeField] public float knockBack { get; protected set; }
    public Stat critical { get; protected set; }
    public Stat criticalDamage { get; protected set; }
    [field: SerializeField] public List<int> statusEffect { get; protected set; }

    Collider2D hitCollider;
    [field: SerializeField] float multiHitDurationTime = 0;

    // 최종 피해량
    public float Damage
    {
        get 
        { 
            if(DamageType == null)
                return DefaultDamage;
            return DefaultDamage + Ratio * DamageType.Value; 
        }
    }

    void Awake()
    {
        hitCollider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (isMultiHit)
        {
            MultiHit();
        }
    }

    #region Set

    // 공격력에 영향을 받지않는 공격
    public void SetHit(float DefaultDamage, float knockBack = 0.0f)
    {
        this.DefaultDamage = DefaultDamage;
        this.knockBack = knockBack;
    }


    public void SetHit_Ratio(
    float DefaultDamage, float Ratio, Stat DamageType, float knockBack = 0.0f, Stat critical = null, Stat criticalDamage = null)
    {
        this.DefaultDamage = DefaultDamage;
        this.Ratio = Ratio;
        this.DamageType = DamageType;

        this.knockBack = knockBack;
        this.critical = critical;
        this.criticalDamage = criticalDamage;
    }

    public void SetProjectile(int penetrations, 
    float DefaultDamage, float Ratio, Stat DamageType, float knockBack = 0.0f, Stat critical = null, Stat criticalDamage = null)
    {
        this.penetrations = penetrations;
        
        this.DefaultDamage = DefaultDamage;
        this.Ratio = Ratio;
        this.DamageType = DamageType;


        this.knockBack = knockBack;
        this.critical = critical;
        this.criticalDamage = criticalDamage;
    }

    public void SetMultiHit(bool isMultiHit, int DPS)
    {
        this.isMultiHit =isMultiHit;
        this.DPS = DPS;
    }

    public void SetSE(int statusEffect)
    {
        this.statusEffect.Clear();
        if(statusEffect < 0 || GameData.instance.statusEffectList.Count < statusEffect)
            return;
        this.statusEffect.Add(statusEffect);
    }

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

    public virtual void SetProjectileTime(float time)
    {
        Destroy(this.gameObject, time);
    }

    #endregion Set

    void MultiHit()
    {
        multiHitDurationTime += Time.deltaTime;
        if (multiHitDurationTime > (1f / DPS))
        {
            StartCoroutine(ColliderOnOff());
            multiHitDurationTime = 0;
        }
    }

    IEnumerator ColliderOnOff()
    {
        hitCollider.enabled = false;
        yield return null;
        hitCollider.enabled = true;
        AttackID = Guid.NewGuid().GetHashCode();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isProjectile)
        {
            if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Door" || other.gameObject.tag == "ShabbyWall"
                 || other.CompareTag("EnemyWall"))
            {
                Destroy(gameObject);
            }

            if (!this.isMultiHit)
            {
                if (this.penetrations >= 1)
                {
                    penetrations--;
                }
                else if (this.penetrations == 0)
                {
                    Destroy(gameObject);
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
        AttackID = Guid.NewGuid().GetHashCode();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
