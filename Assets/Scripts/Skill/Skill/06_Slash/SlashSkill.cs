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

    public override void Enter(ObjectBasic user)
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

            player.stats.MoveSpeed.DecreasedValue += 0.5f;
            holdPower = 1f;

            simul = Instantiate(slashEffectSimul, player.CenterPivot.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;
            simul.transform.localScale = new Vector3(holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, 0);

            while (player.playerStatus.isSkillHold)
            {
                if (holdPower < maxHoldPower && powerTimer > 0.1f)
                {
                    holdPower += 0.025f;
                    simul.transform.localScale = new Vector3(holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, 0);
                    powerTimer = 0;
                }
                simul.transform.rotation = Quaternion.AngleAxis(player.playerStatus.mouseAngle - 90, Vector3.forward);
                powerTimer += Time.deltaTime;
                yield return null;
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float angle = Mathf.Atan2(enemy.enemyStatus.EnemyTarget.transform.position.y - enemy.CenterPivot.transform.position.y, enemy.enemyStatus.EnemyTarget.transform.position.x - enemy.CenterPivot.transform.position.x) * Mathf.Rad2Deg;
            float timer = 0;
            float powerTimer = 0;

            enemy.stats.MoveSpeed.DecreasedValue += 0.5f;
            holdPower = 1f;

            simul = Instantiate(slashEffectSimul, enemy.CenterPivot.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;
            simul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);

            while (timer < maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                if (holdPower < maxHoldPower && powerTimer > 0.1f)
                {
                    holdPower += 0.025f;
                    simul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
                    powerTimer = 0;
                }
                angle = Mathf.Atan2(enemy.enemyStatus.EnemyTarget.transform.position.y - user.transform.position.y, enemy.enemyStatus.EnemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
                simul.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                timer += Time.deltaTime;
                powerTimer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void Cancle()
    {
        StopCoroutine(Simulation());
        // 시뮬 삭제
        // 이동 속도 회복
        Destroy(simul);
        user.GetComponent<Stats>().MoveSpeed.DecreasedValue -= 0.5f;
    }

    public override void Exit()
    {
        StopCoroutine(Simulation());
        Fire();
    }

    void Fire()
    {
        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            Weapon weapon = player.weaponList[player.playerStats.weapon];
            GameObject instantProjectile = Instantiate(slashEffect, player.CenterPivot.transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[weapon.weaponType.ToString()];

            float attackRate = animationInfo.GetSPA() / player.playerStats.attackSpeed;                               // 공격 1회당 걸리는 시간

            player.stats.MoveSpeed.DecreasedValue -= 0.5f;

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            //이펙트 설정
            instantProjectile.transform.rotation = Quaternion.AngleAxis(player.playerStatus.mouseAngle - 90, Vector3.forward);  // 방향 설정
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
            hitDetection.SetHit_Ratio(
                defalutDamage * holdPower, ratio * holdPower, player.stats.AttackPower,
                player.weaponList[player.playerStats.weapon].knockBack * holdPower,
                player.playerStats.CriticalChance,
                player.playerStats.CriticalDamage);
            hitDetection.SetMultiHit(true, (int)((float)DPS / attackRate));
            hitDetection.SetSE((int)player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;
            bulletRigid.velocity = player.playerStatus.mouseDir * 10 * speed;

            Destroy(simul);
            Destroy(instantProjectile, time);  //사거리 설정
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject instantProjectile = Instantiate(slashEffect, enemy.CenterPivot.transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(enemy.enemyStatus.EnemyTarget.transform.position.y - enemy.CenterPivot.transform.position.y, enemy.enemyStatus.EnemyTarget.transform.position.x - enemy.CenterPivot.transform.position.x) * Mathf.Rad2Deg;

            enemy.stats.MoveSpeed.DecreasedValue -= 0.5f;

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
            hitDetection.SetHit_Ratio(defalutDamage, ratio, enemy.stats.AttackPower, 1 * holdPower);
            hitDetection.SetMultiHit(true,DPS);
            hitDetection.user = user;
            bulletRigid.velocity = (enemy.enemyStatus.EnemyTarget.transform.position - enemy.CenterPivot.transform.position).normalized * 10 * speed;  // 속도 설정

            Destroy(simul);
            Destroy(instantProjectile, time);  //사거리 설정
        }
    }
}
