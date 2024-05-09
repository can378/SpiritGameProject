using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    //스탯
    public Stats stats;

    // 이동관련
    public Vector2 moveVec;

    // 피격 관련
    public bool isFlinch;                   // 경직 중
    public Coroutine flinchCoroutine;
    public bool isInvincible;               // 무적 상태

    // 공격 관련 
    public bool isAttack;                   // 공격중
    public float attackDelay;               // 공격중 시간
    public bool isAttackReady;              // 공격 준비 완료
    public GameObject hitTarget;            // 공격 성공

    protected SpriteRenderer sprite;
    protected Rigidbody2D rigid;
    
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

    public virtual void BeAttacked(GameObject attacker)
    {
        if (isInvincible)
        {
            return;
        }

        HitDetection hitDetection = attacker.GetComponent<HitDetection>();

        AudioManager.instance.SFXPlay("Hit_SFX");

        Damaged(hitDetection.damage);

        KnockBack(attacker.gameObject, hitDetection.knockBack);

        if (flinchCoroutine != null) StopCoroutine(flinchCoroutine);
        flinchCoroutine = StartCoroutine(Flinch(0.3f));

        Invincible(0.1f);

        if (hitDetection.statusEffect != null)
        {
            foreach (int statusEffectIndex in hitDetection.statusEffect)
            {
                ApplyBuff(GameData.instance.statusEffectList[statusEffectIndex]);
            }
        }
    }

    public virtual void Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
        bool criticalHit = UnityEngine.Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        Debug.Log(this.gameObject.name + " damaged : " + (1 - stats.defensivePower) * damage);
        stats.HP -= (1 - stats.defensivePower) * damage;

        sprite.color = 0 < (1 - stats.defensivePower) * damage ? Color.red : Color.green;

        Invoke("DamagedOut", 0.05f);
        if (stats.HP <= 0f)
        {
            Dead();
        }
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
        sprite.color = new Color(1, 1, 1, 0.4f);
        Invoke("InvincibleOut", time);
    }

    protected void InvincibleOut()
    {
        //무적 해제
        sprite.color = new Color(1, 1, 1, 1);
        isInvincible = false;
    }

    public void ApplyBuff(GameObject effect)
    {
        // 가지고 있는 버프인지 체크한다.
        StatusEffect statusEffect = effect.GetComponent<StatusEffect>();
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

    public virtual void Dead()
    {
        RemoveAllEffects();
    }

    #endregion
}
