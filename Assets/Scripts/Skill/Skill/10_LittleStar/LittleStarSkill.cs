using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleStarSkill : Skill
{
    // 피해량
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }

    // 기본 크기, 이펙트 유지시간, 이펙트
    [field: SerializeField] float time;
    [field: SerializeField] GameObject LittleStarOrbit;

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        Summon();
    }

    public override void Exit()
    {

    }

    void Summon()
    {
        Debug.Log("Wave");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            GameObject effect = Instantiate(LittleStarOrbit, user.transform.position, user.transform.rotation);
            LittleStarOrbit littleStarOrbit = effect.GetComponent<LittleStarOrbit>();

            // 쿨타임 적용
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            effect.transform.localScale = new Vector3(1, 1, 1);
            effect.tag = "PlayerAttack";
            effect.layer = LayerMask.NameToLayer("PlayerAttack");

            littleStarOrbit.user = user.GetComponent<ObjectBasic>();

            // rate 동안 유지
            Destroy(effect,time);
        }
        else if (user.tag == "Enemy")
        {
            GameObject effect = Instantiate(LittleStarOrbit, user.transform.position, user.transform.rotation);

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;


            effect.transform.localScale = new Vector3(1, 1, 1);
            effect.tag = "EnemyAttack";
            effect.layer = LayerMask.NameToLayer("EnemyAttack");

            // rate 동안 유지
            Destroy(effect);
        }
    }

    
}
