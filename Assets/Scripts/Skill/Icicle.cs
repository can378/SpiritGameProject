using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : Skill
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
    [field: SerializeField] int[] statusEffect;

    //방향 표시기
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

            simul = Instantiate(GameData.instance.simulEffect[2], user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (player.status.isSkillHold)
            {
                // 나중에 원 형태로 최대 범위 제한하기
                // 나중에 원 형태로 최대 범위 표시하기
                simul.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 90, Vector3.forward);
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
        Debug.Log("Icicle");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            // 쿨타임 적용
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            // 이펙트 적용
            GameObject instantProjectile = Instantiate(icicleEffect, transform.position, transform.rotation);
            Destroy(simul);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
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
            hitDetection.SetHitDetection(true, 0, false, -1, defalutDamage + player.stats.skillPower * ratio, knockBack, 0, 0, statusEffect);
            instantProjectile.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 270, Vector3.forward);  // 방향 설정
            bulletRigid.velocity = player.status.mouseDir * 10 * speed;  // 속도 설정
            Destroy(instantProjectile, time);  //사거리 설정
        }
    }
}
