using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectBasic : MonoBehaviour
{
    [HideInInspector] public Stats stats;
    [HideInInspector] public Status status;

    public GameObject[] hitEffects;
    public Transform buffTF;
    public GameObject animGameObject;
    HashSet<int> ReceivedAttackID = new HashSet<int>();

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
        
        if (DuplicateAttack(hitDetection.AttackID))
            return;

        if (status.isInvincible)
            return;

        status.isBeAttaked = true;

        bool criticalHit = false;
        if (hitDetection.critical == null || hitDetection.criticalDamage == null)
            criticalHit = Damaged(hitDetection.Damage);
        else
            criticalHit = Damaged(hitDetection.Damage, hitDetection.critical.Value, hitDetection.criticalDamage.Value);

        // if enemy is Dead, Don't Flinch and Buff
        if(stats.HP <= 0)
            return;


        if (DamagedPoise(hitDetection.Damage))
        {
            //Debug.Log(gameObject.name + ":Flinch");
            SetFlinch(0.5f);

            // 공격을 한 주인이 있다면 그 대상을 중심으로
            if(hitDetection.user != null)
                KnockBack(hitDetection.user.gameObject, hitDetection.knockBack);
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
        if(hitDetection.Damage == 0)
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
        
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.color = Color.red;
        }

        if (status.beAttackedCoroutine != null) StopCoroutine(status.beAttackedCoroutine);
            status.beAttackedCoroutine = StartCoroutine(ChangeHitColor(0.1f));

        #endregion Effect
    }

    // 단순 피해
    public void BeAttacked(float _Damage, Vector3 _HitPos)
    {

        if (status.isInvincible)
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

        #endregion Effect
    }

    public virtual bool Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
      
        if (status.isInvincible)
            return false;

        bool criticalHit = UnityEngine.Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        Debug.Log(this.gameObject.name + " damaged : " + (1 - stats.DefensivePower.Value) * damage);
        stats.HP = Mathf.Min(stats.HP - ((1 - stats.DefensivePower.Value) * damage), stats.HPMax);

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

        StatusEffect statusEffect = stats.activeEffects.Find(x => (int)x.GetType().GetProperty("buffID").GetValue(x) == buffIndex);

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

    public void RemoveBuff(int buffIndex)
    {
        StatusEffect statusEffect = stats.activeEffects.Find(x => (int)x.GetType().GetProperty("buffID").GetValue(x) == buffIndex);

        if (statusEffect)
        {
            statusEffect.Remove();                                  // 버프 해제
            Destroy(statusEffect.gameObject);                       // 버프 아이콘 삭제
            stats.activeEffects.Remove(statusEffect);               // 리스트에서 제거
        }
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
        ReceivedAttackID.Clear();
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
        ReceivedAttackID.Clear();

        foreach (SpriteRenderer sprite in sprites)
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

