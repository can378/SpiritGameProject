using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public GameObject FireBallEffect { get; private set; }
    [field: SerializeField] public GameObject[] StatusEffect { get; private set; }

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
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            // 선딜
            yield return new WaitForSeconds(preDelay / player.stats.attackSpeed);

            GameObject effect = Instantiate(FireBallEffect, player.status.mousePos, Quaternion.identity);
            effect.transform.localScale = new Vector3(size, size, 0);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
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
            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + player.stats.skillPower * ratio, knockBack,0,0, StatusEffect);

            Destroy(effect, rate);
        }
    }

    public override void Exit(GameObject user)
    {

    }
}
