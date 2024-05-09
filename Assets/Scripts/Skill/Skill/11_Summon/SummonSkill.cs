using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkill : Skill
{
    // �߰� ���ݷ� ���, �߰� ����� ���, ��ȯ ���� �ð�
    [field: SerializeField] public float damageRatio { get; private set; }
    [field: SerializeField] public float HPRatio { get; private set; }
    [field: SerializeField] float time;

    // ���� �ð�, ������, ����Ʈ
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
                // ���߿� �� ���·� �ִ� ���� �����ϱ�
                // ���߿� �� ���·� �ִ� ���� ǥ���ϱ�
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
                // ���߿� �� ���·� �ִ� ���� �����ϱ�
                // ���߿� �� ���·� �ִ� ���� ǥ���ϱ�
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

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            // ����Ʈ ����
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

            // rate ���� ����
            Destroy(summon, time);
        }
        else if (user.tag == "Enemy")
        {
            ObjectBasic objectBasic = this.user.GetComponent<ObjectBasic>();
            NPCbasic summonBasic;
            int summonIndex = 0;

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;

            // ����Ʈ ����
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

            // rate ���� ����
            Destroy(summon, time);
        }
    }

    
}
