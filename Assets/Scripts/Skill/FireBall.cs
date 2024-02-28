using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{
    [field: SerializeField] public List<int> attackAttributes { get; private set; }
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public GameObject FireBallEffect { get; private set; }
    [field: SerializeField] public GameObject BurnDeBuff { get; private set; }

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        Debug.Log("FireBall");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime + player.userData.skillCoolTime * skillDefalutCoolTime;

            // 공속 = 플레이어 공속 * 무기 공속
            float attackRate = player.userData.playerAttackSpeed;

            // 선딜
            yield return new WaitForSeconds(preDelay / attackRate);

            GameObject instant = Instantiate(FireBallEffect, player.status.mousePos, Quaternion.identity);
            HitDetection hitDetection = instant.GetComponent<HitDetection>();
            /*
            투사체 = false
            관통력 = -1
            다단히트 = false
            초당 타격 횟수 = -1 
            속성 = 불 : 4
            피해량 = 피해량 * 플레이어 주문력
            넉백 = 넉백
            치확 = 0
            치뎀 = 0
            디버프 = 화상
            */
            hitDetection.SetHitDetection(false, -1, false, -1, attackAttributes, damage * player.userData.skillPower, knockBack,0,0, BurnDeBuff);

            Destroy(instant, rate);

            // 후딜
            yield return new WaitForSeconds(postDelay / attackRate);
        }
    }

    public override void Exit(GameObject user)
    {

    }
}
