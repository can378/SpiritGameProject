using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSkill : Skill
{
    // 피해량
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }

    // 기본 크기, 이펙트 유지시간, 이펙트
    [field: SerializeField] float time;
    [field: SerializeField] GameObject waveEffect;
    [field: SerializeField] int[] statusEffect;

    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        StartCoroutine(Attack());
    }

    public override void Cancle()
    {
        
    }

    public override void Exit()
    {

    }

    IEnumerator Attack()
    {
        Debug.Log("Wave");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            GameObject effect = Instantiate(waveEffect, player.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            effect.transform.localScale = new Vector3(1, 1, 1);
            effect.tag = "PlayerAttack";
            effect.layer = LayerMask.NameToLayer("PlayerAttack");

            /*
            투사체 = false
            관통력 = -1
            다단히트 = false
            초당 타격 횟수 = -1 
            피해량 = (기본 피해량 + 무기 피해량) * 플레이어 공격력
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 없음
            */
            hitDetection.SetHit_Ratio(
             defalutDamage , ratio, player.stats.AttackPower,
             10);
            hitDetection.SetSEs(statusEffect);
            hitDetection.user = user;

            // rate 동안 유지
            Destroy(effect,time);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(waveEffect, enemy.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
            float timer = 0;

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;


            effect.transform.localScale = new Vector3(1, 1, 1);
            effect.tag = "EnemyAttack";
            effect.layer = LayerMask.NameToLayer("EnemyAttack");

            /*
            투사체 = false
            관통력 = -1
            다단히트 = false
            초당 타격 횟수 = -1 
            피해량 = (기본 피해량 + 무기 피해량) * 플레이어 공격력
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 없음
            */
            hitDetection.SetHit_Ratio(
             defalutDamage, ratio, enemy.stats.SkillPower,
             10);
            hitDetection.SetSEs(statusEffect);
            hitDetection.user = user;

            while (timer < time)
            {
                effect.transform.localScale = new Vector3(1 + timer * 10, 1 + timer * 10, 1);
                timer += Time.deltaTime;
                yield return null;
            }

            // rate 동안 유지
            Destroy(effect,time);
        }
    }

    
}
