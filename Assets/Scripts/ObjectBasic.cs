using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    //����
    [HideInInspector] public Stats stats;
    // ���� �ൿ ����
    [HideInInspector] public Status status;

    public GameObject[] hitEffects;           // ���ݹ����� �����ϴ� ��ü
    public int defaultLayer;
    public Transform buffTF;                // ���� ������ ��ġ

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

    }

    #region Effect

    public virtual void BeAttacked(HitDetection hitDetection)
    {
        if (status.isInvincible)
            return;

        AudioManager.instance.SFXPlay("Hit_SFX");

        Damaged(hitDetection.damage, hitDetection.critical, hitDetection.criticalDamage);

        // ���ε��� 0�� �� �� �����ǰ� �˹�
        if(DamagedPoise(hitDetection.damage))
        {
            Debug.Log(gameObject.name + ":Flinch");
            if (status.flinchCoroutine != null) StopCoroutine(status.flinchCoroutine);
            status.flinchCoroutine = StartCoroutine(Flinch(0.5f));

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

        sprite.color = 0 < (1 - stats.defensivePower) * damage ? Color.red : Color.green;

        Invoke("DamagedOut", 0.05f);
    }

    protected virtual void DamagedOut()
    {
        sprite.color = Color.white;
    }

    /// <summary>
    /// ��������
    /// 0 ���ϰ� �Ǹ� �����ǰ� �ִ� ����ġ�� �ʱ�ȭ�ȴ�.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool DamagedPoise(float damage)
    {
        if(status.isInvincible)
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
    /// ���� ȸ��
    /// �ʴ� 2�� ���� ���ظ� ȸ�� �ȴ�.
    /// </summary>
    public void HealPoise()
    {
        stats.poise = Mathf.Min(stats.poise + Time.deltaTime * 1f, stats.poiseMax);
    }

    public void KnockBack(GameObject agent, float knockBack)
    {
        Vector2 dir = (transform.position - agent.transform.position).normalized;
        rigid.AddForce(dir * (knockBack * (1 - stats.defensivePower)), ForceMode2D.Impulse);
    }

    public IEnumerator Flinch(float time = 0)
    {
        status.isFlinch = true;
        AttackCancle();

        yield return new WaitForSeconds(time);

        status.isFlinch = false;
    }

    /// <summary>
    /// ���� ĵ��
    /// ���� �� ���� �� 
    /// ���� �ڷ�ƾ ����, ���� ���� ����
    /// </summary>
    public virtual void AttackCancle()
    {
        status.isAttack = false;
        status.isAttackReady = true;
        status.moveVec = Vector2.zero;

        foreach(GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// ��� ���� �ʱ� ���·� ����
    /// �ش� ������Ʈ�� �ٺ��� �Ǵ� ���� ����
    /// </summary>
    public virtual void InitStatus()
    {
        status.isAttack = false;
        status.isAttackReady = true;
        status.isFlinch = false;
        status.isInvincible = false;
        status.moveVec = Vector2.zero;

        foreach(GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);
    }

    public void Invincible(float time = 0)
    {
        status.isInvincible = true;
        sprite.color = Color.white;
        Invoke("InvincibleOut", time);
    }

    protected void InvincibleOut()
    {
        //���� ����
        sprite.color = new Color(1, 1, 1, 1);
        status.isInvincible = false;
    }

    public void ApplyBuff(int buffIndex)
    {
        if (status.isInvincible)
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

        // ������ �ִ� �������� üũ�Ѵ�.
        foreach (StatusEffect buff in stats.activeEffects)
        {
            // ������ �ִ� ������� �����Ѵ�.
            if (buff.buffId == buffIndex)
            {
                buff.ResetEffect();
                return;
            }
        }

        // ������ �ִ� ������ �ƴ϶�� ���� �߰��Ѵ�.
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

    // ���� ����
    public virtual void Dead()
    {
        print(this.name + " Dead");
        this.gameObject.SetActive(false);
        RemoveAllEffects();
        AttackCancle();
        //Destroy(this.gameObject);
    }

    #endregion
}