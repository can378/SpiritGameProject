using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    [HideInInspector] public Stats stats;
    [HideInInspector] public Status status;

    public Transform CenterPivot;

    public GameObject[] hitEffects;
    public Transform buffTF;
    public GameObject animGameObject;
    HashSet<int> ReceivedAttackID = new HashSet<int>(); // ID의 빠른 추가와 제거, 그리고 중복되어 있는지만 확인하면 되기 때문에 HashSet

    [HideInInspector] AnimBasic animBasic;
    [HideInInspector] public SpriteRenderer[] sprites;
    [HideInInspector] public Rigidbody2D rigid;

    // 이벤트
    // 피격, 버프, 패시브, 공격 등 추가할 예정
    public event System.Action BeAttackedEvent;
    
    protected virtual void Awake()
    {
        sprites = animGameObject.GetComponentsInChildren<SpriteRenderer>(true);
        rigid = GetComponent<Rigidbody2D>();
        animBasic = animGameObject.GetComponent<AnimBasic>();
    }

    protected virtual void LateUpdate()
    {
        status.hitTarget = null;
        status.isBeAttaked = false;
    }

    #region BeAttacked

    // BeAttacked 함수에서 호출함
    bool DuplicateAttack(int _AttackID)
    {
        // 중복 될 시 True 반환
        if(ReceivedAttackID.Contains(_AttackID))
            return true;

        // 중복이 아니면 저장한 후 0.5초 후 삭제
        ReceivedAttackID.Add(_AttackID);
        StartCoroutine(RemoveAttackID(_AttackID));
        return false;
    }

    private IEnumerator RemoveAttackID(int _AttackID)
    {
        yield return new WaitForSeconds(0.5f); // 0.5초 후 다시 피격 가능
        ReceivedAttackID.Remove(_AttackID);

    }

    public void BeAttacked(HitDetection hitDetection, Vector3 _HitPos)
    {

        // 같은 공격에 다시 피격되지 않음
        if (DuplicateAttack(hitDetection.AttackID))
            return;

        // 무적일 시 피격되지 않음
        // 사망 시 피격되지 않음
        if (status.isInvincible || status.isDead)
            return;

        // 피격 상태
        status.isBeAttaked = true;

        // 피해량 계산
        bool criticalHit = false;
        if (hitDetection.critical == null || hitDetection.criticalDamage == null)
            criticalHit = Damaged(hitDetection.Damage);
        else
            criticalHit = Damaged(hitDetection.Damage, hitDetection.critical.Value, hitDetection.criticalDamage.Value);

        // if enemy is Dead, Don't Flinch and Buff
        if (stats.HP <= 0)
            return;

        // 경직 상태 계산
        if (DamagedPoise(hitDetection.Damage))
        {
            //Debug.Log(gameObject.name + ":Flinch");
            SetFlinch(0.5f);

            // 공격을 한 주인이 있다면 그 대상을 중심으로
            if (hitDetection.user != null)
                KnockBack(hitDetection.user.gameObject, hitDetection.knockBack);
            else
                KnockBack(hitDetection.gameObject, hitDetection.knockBack);
        }

        // 상태이상 적용
        if (hitDetection.statusEffect != null)
        {
            foreach (BuffData statusEffect in hitDetection.statusEffect)
            {
                print(statusEffect.buffName);
                ApplyBuff(statusEffect);
            }
        }

        BeAttackedEvent?.Invoke();

        // 피격 시 이펙트 효과
        #region Effect
        if (hitDetection.Damage == 0)
            return;


        if (criticalHit)
        {
            ObjectPoolManager.instance.Get("Hit_Red").transform.position = _HitPos;
            AudioManager.instance.SFXPlay("Stab_Attack_Sound");
        }
        else
        {
            ObjectPoolManager.instance.Get("Hit_White").transform.position = _HitPos;
            AudioManager.instance.SFXPlay("Hit_SFX");
        }

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = Color.red;
        }

        if (status.beAttackedCoroutine != null) StopCoroutine(status.beAttackedCoroutine);
        status.beAttackedCoroutine = StartCoroutine(ChangeHitColor(0.1f));

        transform.DOShakePosition(0.1f, new Vector3(0.1f, 0.1f,0.0f));
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        #endregion Effect
    }

    // 단순 피해
    public void BeAttacked(float _Damage, Vector3 _HitPos)
    {

        if (status.isInvincible || status.isDead)
            return;

        status.isBeAttaked = true;

        Damaged(_Damage);

        // if enemy is Dead, Don't Flinch and Buff
        if (stats.HP <= 0)
            return;

        #region Effect

        ObjectPoolManager.instance.Get("Hit_White").transform.position = _HitPos;
        AudioManager.instance.SFXPlay("Hit_SFX");

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = Color.red;
        }

        if (status.beAttackedCoroutine != null) StopCoroutine(status.beAttackedCoroutine);
        status.beAttackedCoroutine = StartCoroutine(ChangeHitColor(0.1f));

        transform.DOShakePosition(0.1f, 0.1f);

        #endregion Effect
    }

    public virtual bool Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
      
        if (status.isInvincible || status.isDead)
            return false;

        bool criticalHit = UnityEngine.Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        Debug.Log(this.gameObject.name + " damaged : " + (1 - stats.DefensivePower.Value) * damage);
        stats.HP = Mathf.Min(stats.HP - ((1 - stats.DefensivePower.Value) * damage), stats.HPMax.Value);

        if (stats.HP <= 0)
            Dead();

        return criticalHit;
    }

    /// <summary>
    /// Change Color Red
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    protected IEnumerator ChangeHitColor(float time = 0)
    {
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.color = Color.red;
        }

        yield return new WaitForSeconds(time);

        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.color = new Color(1, 1, 1,1);
        }
    }

    #endregion BeAttacked

    #region Flinch
    bool DamagedPoise(float damage)
    {
        if(status.isInvincible || status.isSuperArmor)
            return false;

        stats.poise = Mathf.Min(stats.poise - ((1 - stats.DefensivePower.Value) * damage), stats.poiseMax);

        if(stats.poise <= 0)
        {
            stats.poise = stats.poiseMax;
            return true;
        }
        else
            return false;
    }

    protected void HealPoise()
    {
        // 경직 중이라면
        if( 0 < status.isFlinch)
        {
            status.isFlinch -= Time.deltaTime;
            stats.poise = stats.poiseMax;
            return;
        }

        stats.poise = Mathf.Min(stats.poise + Time.deltaTime * 1f, stats.poiseMax);

    }

    public void KnockBack(GameObject agent, float knockBack)
    {
        Vector2 dir = (transform.position - agent.transform.position).normalized;
        rigid.AddForce(dir * (knockBack * (1 - stats.DefensivePower.Value)), ForceMode2D.Impulse);
    }

    public void SetFlinch(float time = 0)
    {
        FlinchCancle();
    
        if (animBasic != null)
        {
            animBasic.animator.Rebind();
            animBasic.animator.SetTrigger("isHurt");
        }

        status.isFlinch = Mathf.Max(status.isFlinch, time);
    }

    public void ClearFlinch()
    {
        status.isFlinch = 0;
    }


    public virtual void FlinchCancle()
    {
        status.isAttack = false;
        status.isAttackReady = true;
        status.moveVec = Vector2.zero;
        status.moveSpeedMultiplier = 1.0f;
        foreach (GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);
    }

    #endregion Flinch

    #region Invincible

    public void Invincible(float time = 0)
    {
        status.isInvincible = true;
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.color = Color.white;
        }
        Invoke("InvincibleOut", time);
    }

    protected void InvincibleOut()
    {
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }
        status.isInvincible = false;
    }

    #endregion  Invincible

    #region Buff

    public Buff ApplyBuff(BuffData _Buff)
    {
        if (status.isDead)
            return null;

        Buff buff;
        stats.activeEffects.TryGetValue(_Buff.buffID, out buff);

        // 이미 같은 버프가 있다면 중첩 처리
        if (buff != null)
        {
            buff.Overlap();
            return buff;
        }

        buff = new Buff(_Buff, this);

        buff.Apply();
        stats.activeEffects.Add(_Buff.buffID,buff);

        return buff;
    }

    public Buff FindBuff(BuffData _Buff)
    {
        Buff buff;
        stats.activeEffects.TryGetValue(_Buff.buffID, out buff);
        return buff;
    }

    public void RemoveBuff(BuffData _Buff)
    {
        Buff buff;
        stats.activeEffects.TryGetValue(_Buff.buffID, out buff);

        if (buff != null)
        {
            buff.Remove();                                  // 버프 해제
            //Destroy(buff.gameObject);                       // 버프 아이콘 삭제
            stats.activeEffects.Remove(_Buff.buffID);               // 리스트에서 제거
        }
    }

    protected void Update_Buff()
    {
        if (status.isDead)
            return;

        List<int> toRemove = new();

        foreach (var kvp in stats.activeEffects)
        {
            // 지속 시간 종료 시
            Buff buff = kvp.Value;
            if (0 >= buff.curDuration)
            {
                stats.activeEffects[buff.buffData.buffID].Remove();                // 버프 해제
                //Destroy(stats.activeEffects[i].gameObject);     // 버프 아이콘 삭제
                toRemove.Add(buff.buffData.buffID);              // 리스트에서 제거
                continue;
            }
            buff.curDuration -= Time.deltaTime;  // 지속시간 감소
            buff.Update_Buff();                  // 효과
        }

        foreach (int id in toRemove)
            stats.activeEffects.Remove(id);
    }

    public void RemoveAllBuff()
    {
        foreach (var kvp in stats.activeEffects)
        {
            Buff buff = kvp.Value;
            buff.Remove();
            //Destroy(effect.gameObject);
        }
        stats.activeEffects.Clear();
    }

    #endregion Buff

    #region Passive
    
    public PassiveData ApplyPassive(PassiveData _Passive)
    {
        if(status.isDead)
            return null;

        PassiveData passive;
        stats.activePassive.TryGetValue(_Passive.PID, out passive);

        if (passive)
        {
            return passive;
        }
        _Passive.Apply(this);
        stats.activePassive.Add(_Passive.PID,_Passive);

        return passive;
    }

    public PassiveData FindPassive(PassiveData _Passive)
    {
        // 있는지 찾아보고 반환한다.
        PassiveData passive;
        stats.activePassive.TryGetValue(_Passive.PID, out passive);
        return passive;
    }

    public void RemovePassive(PassiveData _Passive)
    {
        PassiveData passive;
        stats.activePassive.TryGetValue(_Passive.PID, out passive);

        if (passive)
        {
            passive.Remove(this);                                   // 버프 해제
            stats.activePassive.Remove(passive.PID);                    // 리스트에서 제거
        }
        else
        {
            Debug.Log("존재하지 않는 패시브 제거");
        }
    }

    protected void Update_Passive()
    {
        if (status.isDead)
            return;

        foreach (var kvp in stats.activePassive)
        {
            // 지속 시간 종료 시
            PassiveData passive = kvp.Value;
            passive.Update_Passive(this);
        }
    }

    public void AddEnchant_SE(SE_TYPE _Type)
    {
        stats.SEType.Insert(0, _Type);
    }

    public void AddEnchant_Common(COMMON_TYPE _Type)
    {
        stats.CommonType.Insert(0, _Type);
    }

    public void AddEnchant_Projectile(PROJECTILE_TYPE _Type)
    {
        stats.ProjectileType.Insert(0, _Type);
    }

    public void RemoveEnchant_SE(SE_TYPE _Type)
    {
        stats.SEType.Remove(_Type);
    }

    public void RemoveEnchant_Common(COMMON_TYPE _Type)
    {
        stats.CommonType.Remove(_Type);
    }

    public void RemoveEnchant_Projectile(PROJECTILE_TYPE _Type)
    {
        stats.ProjectileType.Remove(_Type);
    }
    
    #endregion

    #region Dead

    public virtual void Dead()
    {
        print(this.name + " Dead");

        RemoveAllBuff();
        FlinchCancle();
        StopAllCoroutines();
        ReceivedAttackID.Clear();
        status.isDead = true;
        status.moveVec = new Vector2(0, 0);
        //this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    #endregion Dead
   
    /// <summary>
    /// Reset status Func
    /// </summary>
    public virtual void InitStatus()
    {
        status.isAttack = false;
        status.isAttackReady = true;
        status.isSuperArmor = false;
        status.isFlinch = 0.0f;
        status.isInvincible = false;
        status.moveVec = Vector2.zero;
        ReceivedAttackID.Clear();

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }

        foreach(GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);
        RemoveAllBuff();
    }

    /// <summary>
    /// Easy Color Chanage Func
    /// </summary>
    /// <param name="_Color"></param>
    public void ChangeColor(Color _Color)
    {
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.color = _Color;
        }
    }

    public void ChangeColor(Vector4 _Color)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = _Color;
        }
    }

}

