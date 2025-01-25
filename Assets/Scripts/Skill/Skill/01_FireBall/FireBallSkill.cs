using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : Skill
{
    [Header("Information")]
    [field: SerializeField] int defalutDamage;              // 기본 대미지
    [field: SerializeField] float ratio;                    // 도력 비율

    [field: SerializeField] float knockBack;                // 넉백 거리
    [field: SerializeField] float range;                    // 사정거리

    [Header ("GameObject")]
    [field: SerializeField] GameObject fireBallPrefab;
    [field: SerializeField] GameObject simulPrefab;
    [field: SerializeField] GameObject fireBallSimulPrefab;



    public override void Enter(GameObject user)
    {
        base.Enter(user);
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if(user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            fireBallSimulPrefab.SetActive(true);
            fireBallSimulPrefab.transform.localScale = Vector3.one * 10;

            simulPrefab.SetActive(true);
            simulPrefab.transform.localScale = Vector3.one * range * 2;
            
            while (player.playerStatus.isSkillHold)
            {
                fireBallSimulPrefab.transform.position = player.transform.position + Vector3.ClampMagnitude(player.playerStatus.mousePos - player.transform.position, range);
                yield return null;
            }
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float timer = 0;

            fireBallSimulPrefab.SetActive(true);
            fireBallSimulPrefab.transform.localScale = Vector3.one * 10;

            simulPrefab.SetActive(true);
            simulPrefab.transform.localScale = Vector3.one * range * 2;

            while (timer <= maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                fireBallSimulPrefab.transform.position = enemy.transform.position + Vector3.ClampMagnitude(enemy.enemyStatus.enemyTarget.transform.position - enemy.transform.position, range);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        StopCoroutine("Simulation");
        fireBallSimulPrefab.SetActive(false);
        simulPrefab.SetActive(false);
    }

    public override void Exit()
    {
        StopCoroutine("Simulation");
        Fire();
    }

    void Fire()
    {
        Debug.Log("FireBall");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            GameObject Effect = Instantiate(fireBallPrefab, fireBallSimulPrefab.transform.position,Quaternion.identity);
            HitDetection hitDetection = Effect.GetComponent<HitDetection>();

            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            simulPrefab.SetActive(false);
            fireBallSimulPrefab.SetActive(false);

            Effect.tag = "PlayerAttack";
            Effect.layer = LayerMask.NameToLayer("PlayerAttack");

            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + player.playerStats.skillPower * ratio, knockBack);
            hitDetection.SetSE(3);
            hitDetection.user = user;
            
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();

            GameObject Effect = Instantiate(fireBallPrefab, fireBallSimulPrefab.transform.position, Quaternion.identity);
            HitDetection hitDetection = Effect.GetComponent<HitDetection>();

            skillCoolTime = skillDefalutCoolTime;

            simulPrefab.SetActive(false);
            fireBallSimulPrefab.SetActive(false);

            Effect.tag = "EnemyAttack";
            Effect.layer = LayerMask.NameToLayer("EnemyAttack");

            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + enemy.stats.attackPower * ratio, knockBack);
            hitDetection.SetSE(3);
            hitDetection.user = user;
        }
    }
}
