using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : Skill
{
    // 피해량
    [field: SerializeField] public int defalutDamage {get; private set;}
    [field: SerializeField] public float ratio { get; private set; }

    // 크기, 넉백, 이펙트 유지시간, 이펙트, 상태이상
    [field: SerializeField] float size;
    [field: SerializeField] float knockBack;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject fireBallEffect;
    [field: SerializeField] GameObject fireBallEffectSimul;
    [field: SerializeField] int[] statusEffect;

    //발동 전 효과 범위 표시기
    GameObject simul;
    

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if(user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            simul = Instantiate(fireBallEffectSimul, player.status.mousePos, Quaternion.identity);
            simul.transform.localScale = new Vector3(size, size, 0);
            
            while (player.status.isSkillHold)
            {
                // 나중에 원 형태로 최대 범위 제한하기
                // 나중에 원 형태로 최대 범위 표시하기
                simul.transform.position = player.status.mousePos;
                yield return null;
            }
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float timer = 0;

            simul = Instantiate(fireBallEffectSimul, enemy.enemyTarget.transform.position, Quaternion.identity);
            simul.transform.localScale = new Vector3(size, size, 0);

            while (timer <= maxHoldTime/2)
            {
                // 나중에 원 형태로 최대 범위 제한하기
                // 나중에 원 형태로 최대 범위 표시하기
                simul.transform.position = enemy.enemyTarget.transform.position;
                timer += Time.deltaTime;
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
        Debug.Log("FireBall");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            GameObject effect = Instantiate(fireBallEffect, simul.transform.position, Quaternion.identity);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 쿨타임 적용
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            Destroy(simul);
            Destroy(effect, time);

            effect.transform.localScale = new Vector3(size, size, 0);
            effect.tag = "PlayerAttack";
            effect.layer = LayerMask.NameToLayer("PlayerAttack");

            /*
            투사체 = false
            관통력 = -1
            다단히트 = false
            초당 타격 횟수 = -1 
            피해량 = 피해량 * 플레이어 주문력
            넉백 = 넉백
            치확 = 0
            치뎀 = 0
            디버프 = 화상
            */
            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + player.stats.skillPower * ratio, knockBack, 0, 0, statusEffect);

            
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(fireBallEffect, simul.transform.position, Quaternion.identity);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            skillCoolTime = skillDefalutCoolTime;

            // 이펙트 적용
            Destroy(simul);
            Destroy(effect, time);

            effect.transform.localScale = new Vector3(size, size, 0);
            effect.tag = "EnemyAttack";
            effect.layer = LayerMask.NameToLayer("EnemyAttack");

            /*
            투사체 = false
            관통력 = -1
            다단히트 = false
            초당 타격 횟수 = -1 
            피해량 = 피해량 * 플레이어 주문력
            넉백 = 넉백
            치확 = 0
            치뎀 = 0
            디버프 = 화상
            */
            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + enemy.stats.attackPower * ratio, knockBack, 0, 0, statusEffect);

           
        }
    }
}
