using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : Skill
{
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public float speed {get; private set;}
    [field: SerializeField] public float time { get; private set; }
    [field: SerializeField] public GameObject icicleEffect { get; private set; }
    [field: SerializeField] public GameObject[] statusEffect { get; private set; }

    public override void Enter(GameObject user)
    {
        this.user = user;
    }

    public override void Exit(GameObject user)
    {
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

            GameObject instantProjectile = Instantiate(icicleEffect, transform.position, transform.rotation);
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
