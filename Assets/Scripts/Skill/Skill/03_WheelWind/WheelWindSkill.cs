using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWindSkill : Skill
{
    //피해량
    [field: SerializeField] int defaultDamage;
    [field: SerializeField] float ratio;


    // 초당 타격 횟수, 크기, 이펙트
    [field: SerializeField] int DPS;
    [field: SerializeField] float size;
    [field: SerializeField] GameObject WheelWindPrefab;

    //이펙트
    GameObject WheelWindEffect;

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("WheelWind");

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponList[player.playerStats.weapon];
            HitDetection hitDetection;
            float attackRate = weapon.SPA / player.playerStats.attackSpeed;

            skillCoolTime = 99;

            // 먼저 속도 감소
            player.stats.decreasedMoveSpeed += 0.5f;
            
            // 시간이 조금 지난 후 회전 시작
            yield return new WaitForSeconds(preDelay * attackRate);

            // 사용자 위치에 생성
            if (WheelWindEffect != null)
                Destroy(WheelWindEffect);

            WheelWindEffect = Instantiate(WheelWindPrefab, user.transform.position, user.transform.rotation);
            WheelWindEffect.transform.parent = user.transform;
            WheelWindEffect.transform.localScale = new Vector3(size * player.weaponList[player.playerStats.weapon].attackSize, size * player.weaponList[player.playerStats.weapon].attackSize, 0);
            WheelWindEffect.tag = "PlayerAttack";
            WheelWindEffect.layer = LayerMask.NameToLayer("PlayerAttack");

            // 공격 판정 조정
            hitDetection = WheelWindEffect.GetComponent<HitDetection>();
            /*
            투사체 = false
            관통력 = -1
            다단히트 = true
            초당 타격 횟수 = DPS * attackRate 
            속성 = 무기 속성
            피해량 = (기본 피해량 + 무기 피해량) * 플레이어 공격력
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 없음
            */
            hitDetection.SetHitDetection(false, -1, true, (int)((float)DPS / attackRate),
             defaultDamage + player.stats.attackPower * ratio,
             player.weaponList[player.playerStats.weapon].knockBack,
             player.playerStats.criticalChance,
             player.playerStats.criticalDamage,
             player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            HitDetection hitDetection;

            skillCoolTime = 99;

            // 먼저 속도 감소
            enemy.stats.decreasedMoveSpeed += 0.5f;

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;

            // 시간이 조금 지난 후 회전 시작
            yield return new WaitForSeconds(preDelay);

            // 사용자 위치에 생성
            if (WheelWindEffect != null)
                Destroy(WheelWindEffect);

            WheelWindEffect = Instantiate(WheelWindPrefab, user.transform.position, user.transform.rotation);
            WheelWindEffect.transform.parent = user.transform;
            WheelWindEffect.transform.localScale = new Vector3(size, size, 0);
            WheelWindEffect.tag = "EnemyAttack";
            WheelWindEffect.layer = LayerMask.NameToLayer("EnemyAttack");

            // 공격 판정 조정
            hitDetection = WheelWindEffect.GetComponent<HitDetection>();
            /*
            투사체 = false
            관통력 = -1
            다단히트 = true
            초당 타격 횟수 = DPS * attackRate 
            속성 = 무기 속성
            피해량 = (기본 피해량 + 무기 피해량) * 플레이어 공격력
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 없음
            */
            hitDetection.SetHitDetection(false, -1, true, DPS,
             defaultDamage + enemy.stats.attackPower * ratio,
             1,
             0,
             0,
             null);
            hitDetection.user = user;
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        Destroy(WheelWindEffect);
        user.GetComponent<Stats>().decreasedMoveSpeed -= 0.5f;
    }


    public override void Exit()
    {
        StartCoroutine(AttackOut());
    }

    IEnumerator AttackOut()
    {
        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponList[player.playerStats.weapon];
            float attackRate = weapon.SPA / player.playerStats.attackSpeed;

            yield return new WaitForSeconds(0.5f * attackRate);

            Destroy(WheelWindEffect);

            // 조금 시간이 지난 후 속도 감소 해제
            yield return new WaitForSeconds(postDelay * attackRate);

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            player.stats.decreasedMoveSpeed -= 0.5f;
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();

            yield return new WaitForSeconds(0.5f);

            // 회전 멈춤
            Destroy(WheelWindEffect);

            // 조금 시간이 지난 후 속도 감소 해제
            yield return new WaitForSeconds(postDelay);

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;

            enemy.stats.decreasedMoveSpeed -= 0.5f;
        }
    }

}
