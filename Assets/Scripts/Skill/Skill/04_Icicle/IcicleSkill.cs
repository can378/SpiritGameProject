using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IcicleSkill : Skill
{
    // 피해량
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;

    // 크기, 넉백, 속도, 이펙트 유지시간, 이펙트, 상태이상
    [field: SerializeField] float size;
    [field: SerializeField] float knockBack;
    [field: SerializeField] float speed;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject icicleEffect;
    [field: SerializeField] GameObject fireSimul;
    [field: SerializeField] int[] statusEffect;

    //방향 표시기
    GameObject simul;
    Vector3 simulVector;

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

            if(simul != null)
                Destroy(simul);
            simul = Instantiate(fireSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (player.status.isSkillHold)
            {
                // 나중에 원 형태로 최대 범위 제한하기
                // 나중에 원 형태로 최대 범위 표시하기
                simulVector = player.status.mousePos;
                simul.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 90, Vector3.forward);
                yield return null;
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float angle = Mathf.Atan2(enemy.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
            float timer = 0;

            if (simul != null)
                Destroy(simul);
            simul = Instantiate(fireSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (timer <= maxHoldTime / 2 && enemy.enemyTarget != null )
            {
                // 나중에 원 형태로 최대 범위 제한하기
                // 나중에 원 형태로 최대 범위 표시하기
                angle = Mathf.Atan2(enemy.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
                simulVector = enemy.enemyTarget.transform.position;
                simul.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void Cancle()
    {
        StopCoroutine(Simulation());
        Destroy(simul);
    }

    public override void Exit()
    {
        base.Cancle();
        StopCoroutine(Simulation());
        Fire();
    }

    void Fire()
    {
        Debug.Log("Icicle");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            GameObject instantProjectile = Instantiate(icicleEffect, transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();

            // 쿨타임 적용
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            instantProjectile.transform.localScale = new Vector3(size, size, 0);
            instantProjectile.tag = "PlayerAttack";
            instantProjectile.layer = LayerMask.NameToLayer("PlayerAttack");

            Destroy(simul);
            
            /*
            투사체 = true
            관통력 = 0
            다단히트 = false
            초당 타격 횟수 = -1 
            피해량 = 피해량 * 플레이어 도력
            넉백 = 넉백
            치확 = 0
            치뎀 = 0
            디버프 = 화상
            */
            hitDetection.SetHitDetection(true, 0, false, -1, defalutDamage + player.playerStats.skillPower * ratio, knockBack, 0, 0, statusEffect);
            hitDetection.user = user;
            instantProjectile.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 90, Vector3.forward);  // 방향 설정
            bulletRigid.velocity = (simulVector - user.transform.position).normalized * 10 * speed;  // 속도 설정
            Destroy(instantProjectile, time);  //사거리 설정
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject instantProjectile = Instantiate(icicleEffect, transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(simulVector.y - user.transform.position.y, simulVector.x - user.transform.position.x) * Mathf.Rad2Deg;

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;

            instantProjectile.transform.localScale = new Vector3(size, size,0);
            instantProjectile.tag = "EnemyAttack";
            instantProjectile.layer = LayerMask.NameToLayer("EnemyAttack");

            // 이펙트 적용
            Destroy(simul);


            /*
            투사체 = true
            관통력 = 0
            다단히트 = false
            초당 타격 횟수 = -1 
            피해량 = 피해량 * 플레이어 도력
            넉백 = 넉백
            치확 = 0
            치뎀 = 0
            디버프 = 화상
            */
            hitDetection.SetHitDetection(true, 0, false, -1, defalutDamage + enemy.stats.attackPower * ratio, knockBack, 0, 0, statusEffect);
            hitDetection.user = user;
            instantProjectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);  // 방향 설정
            bulletRigid.velocity = (simulVector - user.transform.position).normalized * 10 * speed;  // 속도 설정
            Destroy(instantProjectile, time);  //사거리 설정
        }
    }
}
