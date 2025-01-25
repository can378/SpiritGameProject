using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    [HideInInspector] public Stats stats;
    [HideInInspector] public Status status;

    public GameObject[] hitEffects;
    public int defaultLayer;
    public Transform buffTF;
    public GameObject animGameObject;

    [HideInInspector] AnimBasic animBasic;
    [HideInInspector] public SpriteRenderer[] sprites;
    [HideInInspector] public Rigidbody2D rigid;
    
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

    public void BeAttacked(HitDetection hitDetection)
    {
        if (status.isInvincible)
            return;

        status.isBeAttaked = true;

       
        bool criticalHit = Damaged(hitDetection.damage, hitDetection.critical, hitDetection.criticalDamage);

        // if enemy is Dead, Don't Flinch and Buff
        if(stats.HP <= 0)
            return;


        if (DamagedPoise(hitDetection.damage))
        {
            Debug.Log(gameObject.name + ":Flinch");
            SetFlinch(0.5f);

            // 공격을 한 주인이 있다면 그 대상을 중심으로
            if(hitDetection.user != null)
                KnockBack(hitDetection.user, hitDetection.knockBack);
            else
                KnockBack(hitDetection.gameObject, hitDetection.knockBack);
        }

        if (hitDetection.statusEffect != null)
        {
            foreach (int statusEffectIndex in hitDetection.statusEffect)
            {
                ApplyBuff(statusEffectIndex);
            }
        }

        #region Effect

        Vector2 oCenter = this.GetComponent<Collider2D>().bounds.center;
        Vector2 hCenter = hitDetection.GetComponent<Collider2D>().bounds.center;
        Vector2 dirVec = (hCenter - oCenter).normalized;
        Vector2 pos = oCenter + dirVec * 0.25f;

        if(criticalHit)
        {
            ObjectPoolManager.instance.Get2("Hit_Red").transform.position = pos;
            AudioManager.instance.SFXPlay("Stab_Attack_Sound");
        }
        else
        {
            ObjectPoolManager.instance.Get2("Hit_White").transform.position = pos;
            AudioManager.instance.SFXPlay("Hit_SFX");
        }
        
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.color = Color.red;
        }

        if (status.beAttackedCoroutine != null) StopCoroutine(status.beAttackedCoroutine);
            status.beAttackedCoroutine = StartCoroutine(ChangeHitColor(0.1f));

        #endregion Effect
    }

    public virtual bool Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
      
        if (status.isInvincible)
            return false;

        bool criticalHit = UnityEngine.Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        //Debug.Log(this.gameObject.name + " damaged : " + (1 - stats.defensivePower) * damage);
        stats.HP = Mathf.Min(stats.HP - ((1 - stats.defensivePower) * damage), stats.HPMax);

        if(stats.HP <= 0)
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

        stats.poise = Mathf.Min(stats.poise - ((1 - stats.defensivePower) * damage), stats.poiseMax);

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
        rigid.AddForce(dir * (knockBack * (1 - stats.defensivePower)), ForceMode2D.Impulse);
    }

    public void SetFlinch(float time = 0)
    {
        AttackCancle();
    
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


    public virtual void AttackCancle()
    {
        status.isAttack = false;
        status.isAttackReady = true;
        status.moveVec = Vector2.zero;

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

    public StatusEffect ApplyBuff(int buffIndex)
    {
        if (status.isInvincible)
            return null;

        StatusEffect statusEffect = stats.activeEffects.Find(x => (int)x.GetType().GetProperty("buffId").GetValue(x) == buffIndex);

        if (statusEffect)
        {
            statusEffect.Overlap();
            return statusEffect;
        }


        GameObject Buff = Instantiate(GameData.instance.statusEffectList[buffIndex], buffTF);
        statusEffect = Buff.GetComponent<StatusEffect>();
        statusEffect.SetTarget(gameObject);

        statusEffect.Apply();
        stats.activeEffects.Add(statusEffect);

        return statusEffect;
    }

    protected void SEProgress()
    {
        for (int i = 0; i < stats.activeEffects.Count();)
        {
            // 지속 시간 종료 시
            if(0 >= stats.activeEffects[i].duration)
            {
                stats.activeEffects[i].Remove();                // 버프 해제
                Destroy(stats.activeEffects[i].gameObject);     // 버프 아이콘 삭제
                stats.activeEffects.RemoveAt(i);                // 리스트에서 제거
                continue;
            }
            stats.activeEffects[i].duration -= Time.deltaTime;  // 지속시간 감소
            stats.activeEffects[i].Progress();                  // 효과
            ++i;
        }
    }

    /*
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
    */
    public void RemoveAllEffects()
    {
        foreach (StatusEffect effect in stats.activeEffects)
        {
            effect.Remove();
            Destroy(effect.gameObject);
        }
        stats.activeEffects.Clear();
    }

    #endregion Buff

    #region Dead

    public virtual void Dead()
    {
        print(this.name + " Dead");
        
        RemoveAllEffects();
        AttackCancle();
        StopAllCoroutines();
        this.gameObject.SetActive(false);
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

        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }

        foreach(GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);
        RemoveAllEffects();
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

}

