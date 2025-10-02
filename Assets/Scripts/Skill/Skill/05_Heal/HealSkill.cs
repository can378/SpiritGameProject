using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : SkillBase
{
    [field: SerializeField] HealSkillData HSData;
    GameObject effect;
    protected void Awake()
    {
        skillData = HSData;
    }
    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        StartCoroutine(HealCoroutine());
    }

    IEnumerator HealCoroutine()
    {
        Debug.Log("Heal");

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();

            // ���� �ӵ� ����
            player.stats.MoveSpeed.DecreasedValue += 0.9f;

            // ����� ��ġ�� ����
            if (effect != null)
                Destroy(effect);
                
            effect = Instantiate(HSData.HealEffectPrefab, user.transform.position, user.transform.rotation);
            effect.transform.parent = user.transform;

            player.Damaged(-player.stats.HPMax.Value * HSData.defaultHeal);

            while(player.playerStatus.isSkillHold)
            {
                player.Damaged(-player.stats.HPMax.Value * HSData.dotHeal * 0.1f);
                yield return new WaitForSeconds(0.1f / player.playerStats.attackSpeed);
            }

        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer = 0;

            // ���� �ӵ� ����
            enemy.stats.MoveSpeed.DecreasedValue += 0.9f;

            // ����� ��ġ�� ����
            if (effect != null)
                Destroy(effect);

            effect = Instantiate(HSData.HealEffectPrefab, user.transform.position, user.transform.rotation);
            effect.transform.parent = user.transform;

            enemy.Damaged(-enemy.stats.HPMax.Value * HSData.defaultHeal);

            while (timer <= HSData.maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                enemy.Damaged(-enemy.stats.HPMax.Value * HSData.dotHeal * 0.1f);
                timer += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    
    public override void Cancle()
    {
        base.Cancle();
        StopCoroutine(HealCoroutine());

        Destroy(effect);

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            player.stats.MoveSpeed.DecreasedValue -= 0.9f;
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * HSData.skillDefalutCoolTime;

        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            enemy.stats.MoveSpeed.DecreasedValue -= 0.9f;
            skillCoolTime = HSData.skillDefalutCoolTime;
        }
    }

    public override void Exit()
    {
        StopCoroutine(HealCoroutine());

        Destroy(effect);

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            player.stats.MoveSpeed.DecreasedValue -= 0.9f;
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * HSData.skillDefalutCoolTime;

        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            enemy.stats.MoveSpeed.DecreasedValue -= 0.9f;
            skillCoolTime = HSData.skillDefalutCoolTime;
        }
    }


}
