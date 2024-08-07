using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : Skill
{
    // 피해량, 밀려남
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;
    [field: SerializeField] float knockBack;

    // 사거리, 크기, 유지시간, v프리팹, 시뮬레이터 프리팹, 상태이상
    [field: SerializeField] float range;
    [field: SerializeField] float size;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject fireBallPrefab;
    [field: SerializeField] GameObject simulPrefab;
    [field: SerializeField] int[] statusEffect;

    //발동 전 효과 범위 표시기
    Transform fireBallSimul;
    Transform rangeSimul;


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

            fireBallSimul = Instantiate(simulPrefab, player.playerStatus.mousePos, Quaternion.identity).transform;
            fireBallSimul.transform.localScale = new Vector3(size, size, 1);

            rangeSimul = Instantiate(simulPrefab,player.transform.position,Quaternion.identity).transform;
            rangeSimul.parent = player.transform;
            rangeSimul.localScale = new Vector3(range * 2 , range * 2, 1);
            
            while (player.playerStatus.isSkillHold)
            {
                fireBallSimul.position = player.transform.position + Vector3.ClampMagnitude(player.playerStatus.mousePos - player.transform.position, range);
                yield return null;
            }
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float timer = 0;

            fireBallSimul = Instantiate(simulPrefab, enemy.enemyStatus.enemyTarget.transform.position, Quaternion.identity).transform;
            fireBallSimul.localScale = new Vector3(size, size, 1);

            rangeSimul = Instantiate(simulPrefab, enemy.transform.position, Quaternion.identity).transform;
            rangeSimul.parent = enemy.transform;
            rangeSimul.localScale = new Vector3(range * 2, range * 2, 1);

            while (timer <= maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                fireBallSimul.position = enemy.transform.position + Vector3.ClampMagnitude(enemy.enemyStatus.enemyTarget.transform.position - enemy.transform.position, range);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        StopCoroutine("Simulation");
        Destroy(rangeSimul.gameObject);
        Destroy(fireBallSimul.gameObject);
    }

    public override void Exit()
    {
        StopCoroutine("Simulation");
        Fire();
    }

    void Fire()
    {
        Debug.Log("FireBall");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            GameObject effect = Instantiate(fireBallPrefab, fireBallSimul.position, Quaternion.identity);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 쿨타임 적용
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            Destroy(rangeSimul.gameObject);
            Destroy(fireBallSimul.gameObject);
            Destroy(effect, time);

            effect.transform.localScale = new Vector3(size, size, 1);
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
            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + player.playerStats.skillPower * ratio, knockBack, 0, 0, statusEffect);
            hitDetection.user = user;
            
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(fireBallPrefab, fireBallSimul.transform.position, Quaternion.identity);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            skillCoolTime = skillDefalutCoolTime;

            // 이펙트 적용
            Destroy(rangeSimul.gameObject);
            Destroy(fireBallSimul.gameObject);
            Destroy(effect, time);

            effect.transform.localScale = new Vector3(size, size, 1);
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
            hitDetection.user = user;
        }
    }
}
