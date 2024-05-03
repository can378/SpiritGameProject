using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoxSkill : Skill
{
    [field: SerializeField] GameObject randomBox;
    [field: SerializeField] GameObject randomBoxSimul;
    [field: SerializeField] int itemRangeMin;
    [field: SerializeField] int itemRangeMax;
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

            simul = Instantiate(randomBoxSimul, player.status.mousePos, Quaternion.identity);
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

            simul = Instantiate(randomBoxSimul, enemy.enemyTarget.transform.position, Quaternion.identity);
            simul.transform.localScale = new Vector3(1, 1, 0);

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
        Summon();
    }

    void Summon()
    {
        Debug.Log("RandomBox Summon");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            // 쿨타임 적용
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;
        }
        else if(user.tag == "Enemy")
        {
            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;
        }

        GameObject box = Instantiate(randomBox, simul.transform.position, Quaternion.identity);

        Destroy(simul);

        List<int> items = box.GetComponent<RewardBox>().items;

        int num = Random.Range(itemRangeMin, itemRangeMax);

        for (int i = 0; i < num; i++)
        {
            int itemCode = Random.Range(1, 5);
            switch (itemCode)
            {
                case 0:
                    break;
                case 1:
                    itemCode = itemCode * 100 + Random.Range(1, GameData.instance.selectItemList.Count);
                    break;
                case 2:
                    itemCode = itemCode * 100 + Random.Range(1, GameData.instance.weaponList.Count);
                    break;
                case 3:
                    itemCode = itemCode * 100 + Random.Range(1, GameData.instance.equipmentList.Count);
                    break;
                case 4:
                    itemCode = itemCode * 100 + Random.Range(1, GameData.instance.skillList.Count);
                    break;
                default:
                    break;
            }
            items.Add(itemCode);
        }
    }
}
