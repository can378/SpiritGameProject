using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{
    [HideInInspector]
    public Transform enemyTarget;
    [HideInInspector]
    public EnemyStats stats;
    [HideInInspector]
    public Rigidbody2D rigid;
    [HideInInspector]
    public SpriteRenderer sprite;
    [HideInInspector]
    public Vector2 targetDirVec;
    [HideInInspector]
    public float targetDis;
    [HideInInspector]
    public float timeValue=0;

    private void Awake()
    {
        enemyTarget = GameObject.FindWithTag("Player").transform;
        rigid = GetComponent<Rigidbody2D>();
        stats = GetComponent<EnemyStats>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        GetComponent<EnemyStats>().isEnemyAttackable = true;
        GetComponent<EnemyStats>().isEnemyMoveable = true;
    }



    private void OnTriggerEnter2D (Collider2D collision) 
    {
        if (collision.tag == "PlayerAttack")
        {
            PlayerAttack(collision.gameObject);
        }
    }

    //Player에게 공격받음
    //Player Attack tag를 가진 object와 충돌 시 사용
    public void PlayerAttack(GameObject attacker) 
    {
        //Damaged
        HitDetection hitDetection = attacker.GetComponent<HitDetection>();

        AudioManager.instance.SFXPlay("Hit_SFX");

        Damaged(hitDetection.damage, hitDetection.critical, hitDetection.criticalDamage);

        if(hitDetection.statusEffect != null)
        {
            foreach (int statusEffectIndex in hitDetection.statusEffect)
            {
                ApplyBuff(GameData.instance.statusEffectList[statusEffectIndex]);
            }
        }

        KnockBack(attacker, hitDetection.knockBack);

    }
    public void Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
        bool criticalHit = Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        print("enemy damaged : " + damage);
        stats.HP -= (1 - stats.addDefensivePower) * damage;

        sprite.color = 0 < (1 - stats.addDefensivePower) * damage ? Color.red : Color.green;

        Invoke("DamagedOut",0.05f);
        if (stats.HP <= 0f)
        {
            Player.instance.stats.exp++;
            MapUIManager.instance.UpdateExpUI();
            EnemyDead();
        }
    }

    void DamagedOut()
    {
        sprite.color = Color.white;
    }

    public void KnockBack(GameObject agent, float knockBack)
    {
        Vector2 dir = (transform.position - agent.transform.position).normalized;
        rigid.AddForce(dir * knockBack, ForceMode2D.Impulse);
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

    private IEnumerator RemoveEffectAfterDuration(StatusEffect effect)
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
        }
        stats.activeEffects.Clear();
    }


    public void EnemyDead()
    {

        //drop coin
        int dropCoinNum = 10;
        GameManager.instance.dropCoin(dropCoinNum, transform.position);

        //enemy disappear
        this.gameObject.SetActive(false);
    }

    public void Chase()
    {
        Vector2 direction = (enemyTarget.position - transform.position).normalized;
        transform.Translate(direction * stats.defaultMoveSpeed * Time.deltaTime);
    }

    public void shot()
    {

        GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
        bullet.transform.position = transform.position;
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(targetDirVec.normalized * 2, ForceMode2D.Impulse);

    }

}
