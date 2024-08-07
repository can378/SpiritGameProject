using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : Skill
{
    // 회복량
    [field: SerializeField] float defaultHeal;
    [field: SerializeField] float dotHeal;


    // 초당 타격 횟수, 크기, 이펙트
    [field: SerializeField] GameObject HealEffect;

    //이펙트
    GameObject effect;

    public override void Enter(GameObject user)
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

            // 먼저 속도 감소
            player.stats.decreasedMoveSpeed += 0.9f;

            // 사용자 위치에 생성
            if (effect != null)
                Destroy(effect);
                
            effect = Instantiate(HealEffect, user.transform.position, user.transform.rotation);
            effect.transform.parent = user.transform;

            player.Damaged(-player.stats.HPMax * defaultHeal);

            while(player.playerStatus.isSkillHold)
            {
                player.Damaged(-player.stats.HPMax * dotHeal * 0.1f);
                yield return new WaitForSeconds(0.1f / player.playerStats.attackSpeed);
            }

        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer = 0;

            // 먼저 속도 감소
            enemy.stats.decreasedMoveSpeed += 0.9f;

            // 사용자 위치에 생성
            if (effect != null)
                Destroy(effect);

            effect = Instantiate(HealEffect, user.transform.position, user.transform.rotation);
            effect.transform.parent = user.transform;

            enemy.Damaged(-enemy.stats.HPMax * defaultHeal);

            while (timer <= maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                enemy.Damaged(-enemy.stats.HPMax * dotHeal * 0.1f);
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
            player.stats.decreasedMoveSpeed -= 0.9f;
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            enemy.stats.decreasedMoveSpeed -= 0.9f;
            skillCoolTime = skillDefalutCoolTime;
        }
    }

    public override void Exit()
    {
        StopCoroutine(HealCoroutine());

        Destroy(effect);

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            player.stats.decreasedMoveSpeed -= 0.9f;
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            enemy.stats.decreasedMoveSpeed -= 0.9f;
            skillCoolTime = skillDefalutCoolTime;
        }
    }


}
