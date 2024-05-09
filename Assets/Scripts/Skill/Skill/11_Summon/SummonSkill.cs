using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkill : Skill
{
    // 추가 공격력 계수, 추가 생명력 계수, 소환 유지 시간
    [field: SerializeField] public float damageRatio { get; private set; }
    [field: SerializeField] public float HPRatio { get; private set; }
    [field: SerializeField] float time;

    // 유지 시간, 프리팹, 이펙트
    [field: SerializeField] GameObject simulPrefab;
    [field: SerializeField] GameObject haetaePrefab;
    [field: SerializeField] GameObject phoenixPrefab;
    [field: SerializeField] GameObject qilinPrefab;
    GameObject simul;
    GameObject summon;

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            simul = Instantiate(simulPrefab, player.status.mousePos, Quaternion.identity);
            simul.transform.localScale = new Vector3(1, 1, 0);

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

            simul = Instantiate(simulPrefab, enemy.enemyTarget.transform.position, Quaternion.identity);
            simul.transform.localScale = new Vector3(1, 1, 0);

            while (timer <= maxHoldTime / 2)
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
        Summon();
    }

    void Summon()
    {
        Debug.Log("Summon");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            NPCbasic summonBasic;
            int summonIndex = 0;

            // 쿨타임 적용
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            // 이펙트 생성
            if(summon)
                Destroy(summon);
            
            switch(summonIndex)
            {
                case 0:
                    summon = Instantiate(haetaePrefab, simul.transform.position, simul.transform.rotation);
                    break;
                case 1:
                    summon = Instantiate(phoenixPrefab, simul.transform.position, simul.transform.rotation);
                    break;
                case 2:
                    summon = Instantiate(qilinPrefab, simul.transform.position, simul.transform.rotation);
                    break;
            }

            Destroy(simul);

            summonBasic = summon.GetComponent<NPCbasic>();
            summonBasic.companionTarget = player;
            summonBasic.side = 1;

            summon.transform.localScale = new Vector3(1, 1, 1);

            summonBasic.stats.HPMax += player.stats.HPMax * HPRatio;
            summonBasic.stats.HP = summonBasic.stats.HPMax;

            summonBasic.stats.defaultAttackPower += player.stats.attackPower * HPRatio;

            // rate 동안 유지
            Destroy(summon, time);
        }
        else if (user.tag == "Enemy")
        {
            ObjectBasic objectBasic = this.user.GetComponent<ObjectBasic>();
            NPCbasic summonBasic;
            int summonIndex = 0;

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;

            // 이펙트 생성
            if (summon)
                Destroy(summon);

            switch (summonIndex)
            {
                case 0:
                    summon = Instantiate(haetaePrefab, simul.transform.position, simul.transform.rotation);
                    break;
                case 1:
                    summon = Instantiate(phoenixPrefab, simul.transform.position, simul.transform.rotation);
                    break;
                case 2:
                    summon = Instantiate(qilinPrefab, simul.transform.position, simul.transform.rotation);
                    break;
            }

            Destroy(simul);

            summonBasic = summon.GetComponent<NPCbasic>();
            summonBasic.companionTarget = objectBasic;
            summonBasic.side = 2;

            summon.transform.localScale = new Vector3(1, 1, 1);

            summonBasic.stats.HPMax += objectBasic.stats.HPMax * HPRatio;
            summonBasic.stats.HP = summonBasic.stats.HPMax;

            summonBasic.stats.defaultAttackPower += objectBasic.stats.attackPower * HPRatio;

            // rate 동안 유지
            Destroy(summon, time);
        }
    }

    
}
