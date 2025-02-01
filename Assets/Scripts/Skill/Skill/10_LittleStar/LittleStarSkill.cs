using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleStarSkill : Skill
{
    // 피해량
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }
    [field: SerializeField] public float knockBack {get; private set;}

    // 유지 시간, 프리팹, 이펙트
    [field: SerializeField] float time;
    [field: SerializeField] GameObject littleStarOrbitPrefab;
    GameObject effect;

    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        Summon();
    }

    public override void Cancle()
    {
        
    }

    public override void Exit()
    {

    }

    void Summon()
    {
        Debug.Log("LittleStar");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            LittleStarOrbit littleStarOrbit;

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            // 이펙트 생성
            if(effect)
                Destroy(effect);
            effect = Instantiate(littleStarOrbitPrefab, user.transform.position, user.transform.rotation);
            effect.transform.localScale = new Vector3(1, 1, 1);

            // 공전 설정
            littleStarOrbit = effect.GetComponent<LittleStarOrbit>();
            littleStarOrbit.user = user.GetComponent<ObjectBasic>();

            foreach (GameObject littleStar in littleStarOrbit.littleStars)
            {
                HitDetection hitDetection = littleStar.GetComponent<HitDetection>();

                littleStar.tag = "PlayerAttack";
                littleStar.layer = LayerMask.NameToLayer("PlayerAttack");
                hitDetection.SetHit_Ratio(defalutDamage, ratio, player.playerStats.SkillPower, knockBack);
            }

            // rate 동안 유지
            Destroy(effect,time);
        }
        else if (user.tag == "Enemy")
        {

            ObjectBasic objectBasic = this.user.GetComponent<ObjectBasic>();
            LittleStarOrbit littleStarOrbit;

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;

            // 이펙트 생성
            if (effect)
                Destroy(effect);
            effect = Instantiate(littleStarOrbitPrefab, user.transform.position, user.transform.rotation);
            effect.transform.localScale = new Vector3(1, 1, 1);

            // 공전 설정
            littleStarOrbit = effect.GetComponent<LittleStarOrbit>();
            littleStarOrbit.user = user.GetComponent<ObjectBasic>();

            foreach (GameObject littleStar in littleStarOrbit.littleStars)
            {
                HitDetection hitDetection = littleStar.GetComponent<HitDetection>();

                littleStar.tag = "EnemyAttack";
                littleStar.layer = LayerMask.NameToLayer("EnemyAttack");
                hitDetection.SetHit_Ratio(defalutDamage, ratio, objectBasic.stats.SkillPower, knockBack);
            }
        }
    }

    
}
