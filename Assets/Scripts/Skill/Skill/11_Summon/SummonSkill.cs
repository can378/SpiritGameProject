using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkill : Skill
{
    // 추가 공격력 계수, 추가 생명력 계수, 소환 유지 시간
    [field: SerializeField] public float damageRatio { get; private set; }
    [field: SerializeField] public float HPRatio { get; private set; }

    // 해태 프리팹, 봉황,주작 프리팹, 기린 프리팹, 시뮬레이션 프리팹
    [field: SerializeField] float time;
    [field: SerializeField] float range;
    [field: SerializeField] GameObject haetaePrefab;
    [field: SerializeField] GameObject phoenixPrefab;
    [field: SerializeField] GameObject qilinPrefab;
    [field: SerializeField] GameObject simulPrefab;

    GameObject summon;
    Transform summonSimul;
    Transform rangeSimul;

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

            summonSimul = Instantiate(simulPrefab, player.status.mousePos, Quaternion.identity).transform;
            summonSimul.transform.localScale = new Vector3(1, 1, 1);

            rangeSimul = Instantiate(simulPrefab, player.transform.position, Quaternion.identity).transform;
            rangeSimul.parent = player.transform;
            rangeSimul.localScale = new Vector3(range * 2, range * 2, 1);

            while (player.status.isSkillHold)
            {
                summonSimul.position = player.transform.position + Vector3.ClampMagnitude(player.status.mousePos - player.transform.position, range);
                yield return null;
            }
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float timer = 0;

            summonSimul = Instantiate(simulPrefab, enemy.enemyTarget.transform.position, Quaternion.identity).transform;
            summonSimul.localScale = new Vector3(1, 1, 1);

            rangeSimul = Instantiate(simulPrefab, enemy.transform.position, Quaternion.identity).transform;
            rangeSimul.parent = enemy.transform;
            rangeSimul.localScale = new Vector3(range * 2, range * 2, 1);

            while (timer <= maxHoldTime / 2)
            {
                // 나중에 원 형태로 최대 범위 제한하기
                // 나중에 원 형태로 최대 범위 표시하기
                summonSimul.position = enemy.transform.position + Vector3.ClampMagnitude(enemy.enemyTarget.transform.position - enemy.transform.position, range);
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
                    summon = Instantiate(haetaePrefab, summonSimul.transform.position, summonSimul.transform.rotation);
                    break;
                case 1:
                    summon = Instantiate(phoenixPrefab, summonSimul.transform.position, summonSimul.transform.rotation);
                    break;
                case 2:
                    summon = Instantiate(qilinPrefab, summonSimul.transform.position, summonSimul.transform.rotation);
                    break;
            }

            Destroy(summonSimul.gameObject);
            Destroy(rangeSimul.gameObject);

            summonBasic = summon.GetComponent<NPCbasic>();
            summonBasic.companionTarget = player;
            summonBasic.side = 1;

            summon.transform.localScale = new Vector3(1, 1, 1);

            summonBasic.stats.HPMax += player.stats.HPMax * HPRatio;
            summonBasic.stats.HP = summonBasic.stats.HPMax;

            summonBasic.stats.defaultAttackPower += player.stats.attackPower * damageRatio;

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
                    summon = Instantiate(haetaePrefab, summonSimul.transform.position, summonSimul.transform.rotation);
                    break;
                case 1:
                    summon = Instantiate(phoenixPrefab, summonSimul.transform.position, summonSimul.transform.rotation);
                    break;
                case 2:
                    summon = Instantiate(qilinPrefab, summonSimul.transform.position, summonSimul.transform.rotation);
                    break;
            }

            Destroy(summonSimul.gameObject);
            Destroy(rangeSimul.gameObject);

            summonBasic = summon.GetComponent<NPCbasic>();
            summonBasic.companionTarget = objectBasic;
            summonBasic.side = 2;

            summon.transform.localScale = new Vector3(1, 1, 1);

            summonBasic.stats.HPMax += objectBasic.stats.HPMax * HPRatio;
            summonBasic.stats.HP = summonBasic.stats.HPMax;

            summonBasic.stats.defaultAttackPower += objectBasic.stats.attackPower * damageRatio;

            // rate 동안 유지
            Destroy(summon, time);
        }
    }

    
}
