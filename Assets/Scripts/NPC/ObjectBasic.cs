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
    public Transform buffTF;

    // 공격 관련
    public bool isAttack;                   // 공격 : 스스로 움직일 수 없으며 추가로 공격 불가
    public bool isAttackReady = true;       // 공격 준비 : false일 시 공격은 할 수 없으나 스스로 이동은 가능
    public GameObject hitTarget;            // 공격 성공
    [field:SerializeField] protected GameObject[] hitEffects;           // 공격범위들 저장하는 객체

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

        // 강인도가 0이 될 시 경직되고 넉백
        if(DamagedPoise(hitDetection.damage))
        {
            Debug.Log(gameObject.name + ":Flinch");
            if (flinchCoroutine != null) StopCoroutine(flinchCoroutine);
            flinchCoroutine = StartCoroutine(Flinch(0.5f));

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
        if(isInvincible)
            return;

        bool criticalHit = UnityEngine.Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        //Debug.Log(this.gameObject.name + " damaged : " + (1 - stats.defensivePower) * damage);
        stats.HP = Mathf.Min(stats.HP - ((1 - stats.defensivePower) * damage), stats.HPMax);

        if(stats.HP <= 0)
            Dead();

        sprite.color = 0 < (1 - stats.defensivePower) * damage ? Color.red : Color.green;

        Invoke("DamagedOut", 0.05f);
    }

    protected virtual void DamagedOut()
    {
        sprite.color = Color.white;
    }

    /// <summary>
    /// 경직피해
    /// 0 이하가 되면 경직되고 최대 경직치로 초기화된다.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool DamagedPoise(float damage)
    {
        if(isInvincible)
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
    public void HealPoise()
    {
        stats.poise = Mathf.Min(stats.poise + Time.deltaTime * 2f, stats.poiseMax);
    }

    public void KnockBack(GameObject agent, float knockBack)
    {
        Vector2 dir = (transform.position - agent.transform.position).normalized;
        rigid.AddForce(dir * (knockBack * (1 - stats.defensivePower)), ForceMode2D.Impulse);
    }

    public IEnumerator Flinch(float time = 0)
    {
        isFlinch = true;
        AttackCancle();

        yield return new WaitForSeconds(time);

        isFlinch = false;
    }

    /// <summary>
    /// 공격 캔슬
    /// 공격 코루틴 해제, 공격 범위 해제
    /// </summary>
    public virtual void AttackCancle()
    {
        isAttack = false;
        isAttackReady = true;
        moveVec = Vector2.zero;
        foreach(GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);
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
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    #endregion
}
