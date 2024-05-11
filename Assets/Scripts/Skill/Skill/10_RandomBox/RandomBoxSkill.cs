using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoxSkill : Skill
{

    [field: SerializeField] float range;
    [field: SerializeField] GameObject randomBoxPrefab;
    [field: SerializeField] GameObject simulPrefab;
    [field: SerializeField] int itemRangeMin;
    [field: SerializeField] int itemRangeMax;

    //�ߵ� �� ȿ�� ���� ǥ�ñ�
    Transform randomBoxSimul;
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

            // ��ȯ�� ��ġ ǥ�ñ�
            randomBoxSimul = Instantiate(simulPrefab, player.status.mousePos, Quaternion.identity).transform;
            randomBoxSimul.localScale = new Vector3(1, 1, 0);

            // ��Ÿ� ǥ�ñ�
            rangeSimul = Instantiate(simulPrefab, player.transform.position, Quaternion.identity).transform;
            rangeSimul.parent = player.transform;
            rangeSimul.localScale = new Vector3(range * 2, range * 2, 1);

            while (player.status.isSkillHold)
            {
                randomBoxSimul.position = player.transform.position + Vector3.ClampMagnitude(player.status.mousePos - player.transform.position, range);
                yield return null;
            }
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float timer = 0;

            randomBoxSimul = Instantiate(simulPrefab, enemy.enemyTarget.transform.position, Quaternion.identity).transform;
            randomBoxSimul.localScale = new Vector3(1, 1, 0);

            // ��Ÿ� ǥ�ñ�
            rangeSimul = Instantiate(simulPrefab, enemy.transform.position, Quaternion.identity).transform;
            rangeSimul.parent = enemy.transform;
            rangeSimul.localScale = new Vector3(range * 2, range * 2, 1);

            while (timer <= maxHoldTime/2)
            {
                randomBoxSimul.position = enemy.transform.position + Vector3.ClampMagnitude(enemy.enemyTarget.transform.position - enemy.transform.position, range);
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

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;
        }
        else if(user.tag == "Enemy")
        {
            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;
        }

        GameObject box = Instantiate(randomBoxPrefab, randomBoxSimul.position, Quaternion.identity);

        Destroy(randomBoxSimul.gameObject);
        Destroy(rangeSimul.gameObject);

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
