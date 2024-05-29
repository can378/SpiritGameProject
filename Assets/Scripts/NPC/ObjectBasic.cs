using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    //스탯
    [HideInInspector] public Stats stats;
    public int defaultLayer;

    // 이동관련
    public Vector2 moveVec;

    // 피격 관련
    public bool isFlinch;                   // 경직 : 스스로 움직일 수 없으며 공격할 수 없음
    public Coroutine flinchCoroutine;
    public bool isInvincible;               // 무적 : 피해와 적의 공격 무시
    public float deadDelay = 0;

    // 공격 관련 
    public bool isAttack;                   // 공격 : 스스로 움직일 수 없으며 추가로 공격 불가
    public bool isAttackReady = true;       // 공격 준비 : false일 시 공격은 할 수 없으나 스스로 이동은 가능
    public GameObject hitTarget;            // 공격 성공
    //public float attackDelay;               // 공격중 시간

    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public Rigidbody2D rigid;
    
    protected virtual void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void LateUpdate()
    {
        hitTarget = null;
    }

    #region Effect

    public virtual void BeAttacked(HitDetection hitDetection)
    {
        if (isInvincible)
            return;

        AudioManager.instance.SFXPlay("Hit_SFX");

        Damaged(hitDetection.damage, hitDetection.critical, hitDetection.criticalDamage);

        if (flinchCoroutine != null) StopCoroutine(flinchCoroutine);
        flinchCoroutine = StartCoroutine(Flinch(0.3f));

        KnockBack(hitDetection.gameObject, hitDetection.knockBack);

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
        if(isInvincible)
            return;

        bool criticalHit = UnityEngine.Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        Debug.Log(this.gameObject.name + " damaged : " + (1 - stats.defensivePower) * damage);
        stats.HP = Mathf.Clamp(stats.HP - ((1 - stats.defensivePower) * damage), 0,stats.HPMax);

        sprite.color = 0 < (1 - stats.defensivePower) * damage ? Color.red : Color.green;

        Invoke("DamagedOut", 0.05f);
    }

    protected virtual void DamagedOut()
    {
        sprite.color = Color.white;
    }

    public void KnockBack(GameObject agent, float knockBack)
    {
        Vector2 dir = (transform.position - agent.transform.position).normalized;
        rigid.AddForce(dir * (knockBack * (1 - stats.defensivePower)), ForceMode2D.Impulse);
    }

    public IEnumerator Flinch(float time = 0)
    {
        isFlinch = true;

        yield return new WaitForSeconds(time);

        isFlinch = false;
    }

    public void Invincible(float time = 0)
    {
        isInvincible = true;
        sprite.color = Color.white;
        Invoke("InvincibleOut", time);
    }

    protected void InvincibleOut()
    {
        //무적 해제
        sprite.color = new Color(1, 1, 1, 1);
        isInvincible = false;
    }

    public void ApplyBuff(int buffIndex)
    {
        if (isInvincible)
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
        

        GameObject effect = GameData.instance.statusEffectList[buffIndex];

        // 가지고 있는 버프인지 체크한다.
        StatusEffect statusEffect = Instantiate(effect, Vector3.zero,Quaternion.identity).GetComponent<StatusEffect>();
        foreach (StatusEffect buff in stats.activeEffects)
        {
            // 가지고 있는 버프라면 갱신한다.
            if (buff.buffId == statusEffect.buffId)
            {
                buff.ResetEffect();
                return;
            }
        }

        // 가지고 있는 버프가 아니라면 새로 추가한다.
        GameObject Buff = Instantiate(effect);
        statusEffect = Buff.GetComponent<StatusEffect>();
        statusEffect.SetTarget(gameObject);

        statusEffect.ApplyEffect();
        stats.activeEffects.Add(statusEffect);

        StartCoroutine(RemoveEffectAfterDuration(statusEffect));
    }

    public void ApplyBuff(GameObject effect)
    {
        if (isInvincible)
            return;

        if (effect.GetComponent<StatusEffect>().buffId <=10 &&  0 < stats.SEResist(effect.GetComponent<StatusEffect>().buffId) )
        {
            // 저주 디버프일 시 피해를 입고 사라짐
            if (effect.GetComponent<StatusEffect>().buffId == 10)
            {
                Damaged(stats.HPMax * 0.1f);
            }
            return;
        }
        

        StatusEffect statusEffect = effect.GetComponent<StatusEffect>();

        // 가지고 있는 버프인지 체크한다.
        foreach (StatusEffect buff in stats.activeEffects)
        {
            // 가지고 있는 버프라면 갱신한다.
            if (buff.buffId == statusEffect.buffId)
            {
                buff.ResetEffect();
                return;
            }
        }

        // 가지고 있는 버프가 아니라면 새로 추가한다.
        GameObject Buff = Instantiate(effect);
        statusEffect = Buff.GetComponent<StatusEffect>();
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

    // 빈사 상태로
    // 회복되면 죽지 않음
    protected void HalfDead()
    {
        if (stats.HP > 0 && deadDelay == 0)
            return;

        isFlinch = true;
        isInvincible = true;
        //StopAllCoroutines();
        deadDelay += Time.deltaTime;

        if(deadDelay >= 5f && stats.HP <= 0)
            Dead();
        else if (deadDelay >= 5f && 0 < stats.HP)
        {
            isFlinch = false;
            isInvincible = false;
            deadDelay = 0f;
        }
            


    }

    // 완전 죽음
    public virtual void Dead()
    {
        print(this.name + " Dead");
        Destroy(this.gameObject);
    }

    #endregion
}
