using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    //����
    [HideInInspector] public Stats stats;
    public int defaultLayer;

    // �̵�����
    public Vector2 moveVec;

    // �ǰ� ����
    public bool isFlinch;                   // ���� : ������ ������ �� ������ ������ �� ����
    public Coroutine flinchCoroutine;
    public bool isInvincible;               // ���� : ���ؿ� ���� ���� ����
    public float deadDelay = 0;

    // ���� ���� 
    public bool isAttack;                   // ���� : ������ ������ �� ������ �߰��� ���� �Ұ�
    public bool isAttackReady = true;       // ���� �غ� : false�� �� ������ �� �� ������ ������ �̵��� ����
    public GameObject hitTarget;            // ���� ����
    //public float attackDelay;               // ������ �ð�

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
        //���� ����
        sprite.color = new Color(1, 1, 1, 1);
        isInvincible = false;
    }

    public void ApplyBuff(int buffIndex)
    {
        if (isInvincible)
            return;

        if (buffIndex <= 10 && 0 < stats.SEResist(buffIndex))
        {
            // ���� ������� �� ���ظ� �԰� �����
            if (buffIndex == 10)
            {
                Damaged(stats.HPMax * 0.1f);
            }
            return;
        }
        

        GameObject effect = GameData.instance.statusEffectList[buffIndex];

        // ������ �ִ� �������� üũ�Ѵ�.
        StatusEffect statusEffect = Instantiate(effect, Vector3.zero,Quaternion.identity).GetComponent<StatusEffect>();
        foreach (StatusEffect buff in stats.activeEffects)
        {
            // ������ �ִ� ������� �����Ѵ�.
            if (buff.buffId == statusEffect.buffId)
            {
                buff.ResetEffect();
                return;
            }
        }

        // ������ �ִ� ������ �ƴ϶�� ���� �߰��Ѵ�.
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
            // ���� ������� �� ���ظ� �԰� �����
            if (effect.GetComponent<StatusEffect>().buffId == 10)
            {
                Damaged(stats.HPMax * 0.1f);
            }
            return;
        }
        

        StatusEffect statusEffect = effect.GetComponent<StatusEffect>();

        // ������ �ִ� �������� üũ�Ѵ�.
        foreach (StatusEffect buff in stats.activeEffects)
        {
            // ������ �ִ� ������� �����Ѵ�.
            if (buff.buffId == statusEffect.buffId)
            {
                buff.ResetEffect();
                return;
            }
        }

        // ������ �ִ� ������ �ƴ϶�� ���� �߰��Ѵ�.
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

    // ��� ���·�
    // ȸ���Ǹ� ���� ����
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

    // ���� ����
    public virtual void Dead()
    {
        print(this.name + " Dead");
        Destroy(this.gameObject);
    }

    #endregion
}
