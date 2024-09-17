using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    //스탯
    [HideInInspector] public Stats stats;
    // 현재 행동 상태
    [HideInInspector] public Status status;

    public GameObject[] hitEffects;           // 공격범위들 저장하는 객체
    public int defaultLayer;
    public Transform buffTF;                // 버프 아이콘 위치

    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public Rigidbody2D rigid;
    
    protected virtual void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void LateUpdate()
    {
        status.hitTarget = null;
        status.isBeAttaked = false;
    }

    #region Effect

    public void BeAttacked(HitDetection hitDetection)
    {
        if (status.isInvincible)
            return;

        status.isBeAttaked = true;

        AudioManager.instance.SFXPlay("Hit_SFX");

        Damaged(hitDetection.damage, hitDetection.critical, hitDetection.criticalDamage);

        // 강인도가 0이 될 시 경직되고 넉백
        if(DamagedPoise(hitDetection.damage))
        {
            Debug.Log(gameObject.name + ":Flinch");
            if (status.flinchCoroutine != null) StopCoroutine(status.flinchCoroutine);
            SetFlinch(0.5f);

            KnockBack(hitDetection.gameObject, hitDetection.knockBack);
        }

        //Invincible(0.1f);

        if (hitDetection.statusEffect != null)
        {
            foreach (int statusEffectIndex in hitDetection.statusEffect)
            {
                ApplyBuff(statusEffectIndex);
            }
        }
    }

    public virtual void Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
        if(status.isInvincible)
            return;

        bool criticalHit = UnityEngine.Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        //Debug.Log(this.gameObject.name + " damaged : " + (1 - stats.defensivePower) * damage);
        stats.HP = Mathf.Min(stats.HP - ((1 - stats.defensivePower) * damage), stats.HPMax);

        if(stats.HP <= 0)
            Dead();
    }

    /// <summary>
    /// 경직피해
    /// 0 이하가 되면 경직되고 최대 경직치로 초기화된다.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    bool DamagedPoise(float damage)
    {
        if(status.isInvincible || status.isSuperArmor)
            return false;

        stats.poise = Mathf.Min(stats.poise - ((1 - stats.defensivePower) * damage), stats.poiseMax);

        if(stats.poise <= 0)
        {
            stats.poise = stats.poiseMax;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 경직 회복
    /// 초당 2로 경직 피해를 회복 된다.
    /// </summary>
    protected void HealPoise()
    {
        stats.poise = Mathf.Min(stats.poise + Time.deltaTime * 1f, stats.poiseMax);
    }

    public void KnockBack(GameObject agent, float knockBack)
    {
        Vector2 dir = (transform.position - agent.transform.position).normalized;
        rigid.AddForce(dir * (knockBack * (1 - stats.defensivePower)), ForceMode2D.Impulse);
    }

    public void SetFlinch(float time = 0)
    {
        status.flinchCoroutine =  StartCoroutine(Flinch(time));
    }

    protected IEnumerator Flinch(float time = 0)
    {
        status.isFlinch = true;
        AttackCancle();

        yield return new WaitForSeconds(time);

        status.isFlinch = false;
    }

    /// <summary>
    /// 공격 캔슬
    /// 공격 중 경직 시 
    /// 공격 코루틴 해제, 공격 범위 해제
    /// </summary>
    public virtual void AttackCancle()
    {
        status.isAttack = false;
        status.isAttackReady = true;
        status.moveVec = Vector2.zero;

        sprite.color = new Color(1f, 1f, 1f, 1f);

        foreach (GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);
    }

    /// <summary>
    /// 상태 초기화
    /// 모든 상태 초기 상태로 변경
    /// 해당 오브젝트가 바보가 되는 것을 방지
    /// </summary>
    public virtual void InitStatus()
    {
        status.isAttack = false;
        status.isAttackReady = true;
        status.isSuperArmor = false;
        status.isFlinch = false;
        status.isInvincible = false;
        status.moveVec = Vector2.zero;

        sprite.color = new Color(1f,1f,1f,1f);

        foreach(GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);
        RemoveAllEffects();
    }

    public void Invincible(float time = 0)
    {
        status.isInvincible = true;
        sprite.color = Color.white;
        Invoke("InvincibleOut", time);
    }

    protected void InvincibleOut()
    {
        //무적 해제
        sprite.color = new Color(1, 1, 1, 1);
        status.isInvincible = false;
    }

    public void ApplyBuff(int buffIndex)
    {
        if (status.isInvincible)
            return;

        if (buffIndex <= 10 && 0 < stats.SEResist(buffIndex))
        {
            // 저주 디버프일 시 피해를 입고 사라짐
            if (buffIndex == 10)
            {
                Damaged(stats.HPMax * 0.1f);
            }
            return;
        }

        // 가지고 있는 버프인지 체크한다.
        foreach (StatusEffect buff in stats.activeEffects)
        {
            // 가지고 있는 버프라면 갱신한다.
            if (buff.buffId == buffIndex)
            {
                buff.ResetEffect();
                return;
            }
        }

        // 가지고 있는 버프가 아니라면 새로 추가한다.
        GameObject Buff = Instantiate(GameData.instance.statusEffectList[buffIndex], buffTF);
        StatusEffect statusEffect = Buff.GetComponent<StatusEffect>();
        statusEffect.SetTarget(gameObject);

        statusEffect.ApplyEffect();
        stats.activeEffects.Add(statusEffect);

        StartCoroutine(RemoveEffectAfterDuration(statusEffect));
    }

    IEnumerator RemoveEffectAfterDuration(StatusEffect effect)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            effect.duration -= 0.1f;
            if (effect.duration <= 0)
            {
                break;
            }
        }
        effect.RemoveEffect();
        stats.activeEffects.Remove(effect);

        Destroy(effect.gameObject);
    }

    public void RemoveAllEffects()
    {
        foreach (StatusEffect effect in stats.activeEffects)
        {
            effect.RemoveEffect();
            Destroy(effect.gameObject);
        }
        stats.activeEffects.Clear();
    }

    // 완전 죽음
    public virtual void Dead()
    {
        print(this.name + " Dead");
        
        RemoveAllEffects();
        AttackCancle();
        StopAllCoroutines();
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    #endregion
}
