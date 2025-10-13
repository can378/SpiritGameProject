using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSkill : SkillBase
{
    [field: SerializeField] WaveSkillData WSData;
    protected void Awake()
    {
        skillData = WSData;
    }
    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        Attack();
    }

    public override void Cancle()
    {
        
    }

    public override void Exit()
    {

    }

    void Attack()
    {
        Debug.Log("Wave");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            GameObject effect = Instantiate(WSData.waveEffectPrefab, player.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * WSData.skillDefalutCoolTime;

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
             WSData.defaultDamage, WSData.ratio, player.stats.AttackPower,
             10);
            hitDetection.SetSE(WSData.statusEffect);
            hitDetection.user = user;

            // Growing 모듈
            hitDetection.SetGrowing(true, 3);

            // rate 동안 유지
            Destroy(effect, WSData.effectTime);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(WSData.waveEffectPrefab, enemy.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 쿨타임 적용
            skillCoolTime = 5;


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
             WSData.defaultDamage, WSData.ratio, enemy.stats.SkillPower,
             10);
            hitDetection.SetSE(WSData.statusEffect);
            hitDetection.user = user;

            // Growing 모듈
            hitDetection.SetGrowing(true, 3);

            // rate 동안 유지
            Destroy(effect, WSData.effectTime);
        }
    }

    
}
