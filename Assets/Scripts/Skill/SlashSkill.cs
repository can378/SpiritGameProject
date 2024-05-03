using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlashSkill : Skill
{
    // 피해량
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;

    // 최대 강화량
    [field: SerializeField] public float maxHoldPower { get; private set; }

    // 초당 타격 횟수, 기본 크기, 속도, 사거리
    [field: SerializeField] int DPS;
    [field: SerializeField] float size;
    [field: SerializeField] float speed;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject slashEffect;
    [field: SerializeField] GameObject slashEffectSimul;

    //방향 표시기
    float holdPower;
    GameObject simul;

    public override void Enter(GameObject user)
    {
        this.user = user;
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            float powerTimer = 0;

            player.stats.decreasedMoveSpeed += 0.5f;
            holdPower = 1f;

            simul = Instantiate(slashEffectSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;
            simul.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.playerStats.weapon].attackSize, 0);

            while (player.status.isSkillHold)
            {
                if (holdPower < maxHoldPower && powerTimer > 0.1f)
                {
                    holdPower += 0.025f;
                    simul.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.playerStats.weapon].attackSize, 0);
                    powerTimer = 0;
                }
                simul.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 90, Vector3.forward);
                powerTimer += Time.deltaTime;
                yield return null;
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float angle = Mathf.Atan2(enemy.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
            float timer = 0;
            float powerTimer = 0;

            enemy.stats.decreasedMoveSpeed += 0.5f;
            holdPower = 1f;

            simul = Instantiate(slashEffectSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;
            simul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);

            while (timer < maxHoldTime / 2)
            {
                if (holdPower < maxHoldPower && powerTimer > 0.1f)
                {
                    holdPower += 0.025f;
                    simul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
                    powerTimer = 0;
                }
                angle = Mathf.Atan2(enemy.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
                simul.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                timer += Time.deltaTime;
                powerTimer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void Exit()
    {
        StopCoroutine(Simulation());
        Fire();
    }

    void Fire()
    {
        Debug.Log("Slash");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            Weapon weapon = player.weaponController.weaponList[player.playerStats.weapon];
            GameObject instantProjectile = Instantiate(slashEffect, transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            float attackRate = weapon.SPA / player.playerStats.attackSpeed;                               // 공격 1회당 걸리는 시간

            player.stats.decreasedMoveSpeed -= 0.5f;

            // 쿨타임 적용
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            //이펙트 설정
            instantProjectile.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 90, Vector3.forward);  // 방향 설정
            instantProjectile.transform.localScale = simul.transform.lossyScale;
            instantProjectile.tag = "PlayerAttack";
            instantProjectile.layer = LayerMask.NameToLayer("PlayerAttack");

            /*
            투사체 = true
            관통력 = -1
            다단히트 = true
            초당 타격 횟수 = DPS / 공격 1회당 걸리는 시간
            피해량 = (기본 피해량 + 공격력 * 계수) * 강화 수치
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 무기 상태이상
            */
            hitDetection.SetHitDetection(true, -1, true, (int)((float)DPS / attackRate),
                (int)((defalutDamage + player.stats.attackPower * ratio) * holdPower),
                player.weaponController.weaponList[player.playerStats.weapon].knockBack * holdPower,
                player.playerStats.criticalChance,
                player.playerStats.criticalDamage,
                player.weaponController.weaponList[player.playerStats.weapon].statusEffect);
            bulletRigid.velocity = player.status.mouseDir * 10 * speed;

            Destroy(simul);
            Destroy(instantProjectile, time);  //사거리 설정
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject instantProjectile = Instantiate(slashEffect, transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(enemy.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;

            enemy.stats.decreasedMoveSpeed -= 0.5f;

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;

            // 이펙트 살정
            instantProjectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);  // 방향 설정
            instantProjectile.transform.localScale = simul.transform.lossyScale;
            instantProjectile.tag = "EnemyAttack";
            instantProjectile.layer = LayerMask.NameToLayer("EnemyAttack");
            /*
            투사체 = true
            관통력 = -1
            다단히트 = true
            초당 타격 횟수 = DPS
            피해량 = (기본 피해량 + 공격력 * 계수) * 강화 수치
            넉백 = 기본 넉백
            치확 = 0
            치뎀 = 0
            디버프 = 0
            */
            hitDetection.SetHitDetection(true, -1, true, DPS,
                (int)((defalutDamage + enemy.stats.attackPower * ratio) * holdPower),
                1 * holdPower,
                0,
                0,
                null);
            bulletRigid.velocity = (enemy.enemyTarget.transform.position - transform.position).normalized * 10 * speed;  // 속도 설정

            Destroy(simul);
            Destroy(instantProjectile, time);  //사거리 설정
        }
    }
}
