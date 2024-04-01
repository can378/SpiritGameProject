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
        sprite = GetComponent<SpriteRenderer>();
    }



    private void OnTriggerEnter2D (Collider2D collision) 
    {
        if (collision.tag == "PlayerAttack")
        {
            PlayerAttack(collision.gameObject);
        }
    }
    //Player Attack tag�� ���� object�� �浹 �� ���
    public void PlayerAttack(GameObject attacker) 
    {
        //Damaged
        HitDetection hitDetection = attacker.GetComponent<HitDetection>();

        AudioManager.instance.SFXPlay("Hit_SFX");

        Damaged(hitDetection.damage, hitDetection.critical, hitDetection.criticalDamage, hitDetection.attackAttributes);
        if (hitDetection.deBuff != null) ApplyBuff(hitDetection.deBuff);
        KnockBack(attacker, hitDetection.knockBack);

    }
    public void Damaged(float damage, float critical = 0, float criticalDamage = 0, List<int> attackAttributes = null)
    {
        bool criticalHit = Random.Range(0, 100) < critical * 100 ? true : false;
        damage = criticalHit ? damage * criticalDamage : damage;

        if(attackAttributes != null)
        {
            foreach (int attackAttribute in attackAttributes)
            {
                string att;
                switch (attackAttribute)
                {
                    case 1: att = "����"; break;
                    case 2: att = "Ÿ��"; break;
                    case 3: att = "����"; break;
                    case 4: att = "ȭ��"; break;
                    case 5: att = "�ñ�"; break;
                    case 6: att = "����"; break;
                    case 7: att = "����"; break;
                    case 8: att = "�ż�"; break;
                    case 9: att = "���"; break;
                    default: att = "���Ӽ�"; break;

                }
                float trueDamage = (damage * (1 - stats.resist[attackAttribute]) / attackAttributes.Count) > 0f ? (damage * (1 - stats.resist[attackAttribute]) / attackAttributes.Count) : 1f;
                print(att + " enemy damaged : " + trueDamage);
                stats.HP -= trueDamage;
            }
        }
        else
        {
            print("enemy damaged : " + damage);
            stats.HP -= damage;
        }
        
        sprite.color = Color.red;
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
        // ������ �ִ� �������� üũ�Ѵ�.
        StatusEffect statusEffect = effect.GetComponent<StatusEffect>();
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

    private IEnumerator RemoveEffectAfterDuration(StatusEffect effect)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
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
        Vector2 direction = enemyTarget.position - transform.position;
        transform.Translate(direction * stats.defaultMoveSpeed * Time.deltaTime);

    }

    public void shot()
    {

        GameObject bullet = ObjectPoolManager.instance.Get(0);
        bullet.transform.position = transform.position;
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(targetDirVec.normalized * 2, ForceMode2D.Impulse);

    }

}
