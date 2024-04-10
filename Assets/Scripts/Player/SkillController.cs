using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats stats;

    // ��ų ����Ʈ
    public Skill[] skillList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        stats = GetComponent<PlayerStats>();
    }

    // ��ų ȹ��
    public void EquipSkill(int skillID)
    {
        stats.skill[status.skillIndex] = skillID;
        skillList[skillID].gameObject.SetActive(true);
        MapUIManager.instance.UpdateSkillUI();

    }

    // ��ų ����
    public void UnEquipSkill()
    {
        Instantiate(DataManager.instance.gameData.skillList[stats.skill[status.skillIndex]], gameObject.transform.position, gameObject.transform.localRotation);
        skillList[stats.skill[status.skillIndex]].gameObject.SetActive(false);
        stats.skill[status.skillIndex] = 0;
        MapUIManager.instance.UpdateSkillUI();
    }

    // ��ųŰ �Է�
    public void SkillDown()
    {
        if (skillList[stats.skill[status.skillIndex]].skillType == 0)
        {
            // ���
            Debug.Log("��ų ��� ����");
            StartCoroutine("Immediate");
        }
        else if (skillList[stats.skill[status.skillIndex]].skillType == 1)
        {
            //�غ�
            Debug.Log("��ų �غ�");
            StartCoroutine("Ready");
        }
        else if (skillList[stats.skill[status.skillIndex]].skillType == 2)
        {
            //Ȧ��
            Debug.Log("��ų Ȧ��");
            StartCoroutine(Hold());
        }

    }

    public IEnumerator Immediate()
    {
        Debug.Log("��ų ����");
        status.isSkillReady = false;
        status.isSkill = true;

        skillList[stats.skill[status.skillIndex]].Use(gameObject);


        // ��ų ���� �ð� (���� �����̱� ���� ��� �ð�)
        float skillUsedTime = skillList[stats.skill[status.skillIndex]].preDelay + skillList[stats.skill[status.skillIndex]].rate + skillList[stats.skill[status.skillIndex]].postDelay;

        if (skillList[stats.skill[status.skillIndex]].skillLimit == SkillLimit.None)
        {
            yield return new WaitForSeconds(skillUsedTime / stats.attackSpeed);
        }
        else
        {
            yield return new WaitForSeconds(skillUsedTime / stats.attackSpeed);
        }

        status.isSkill = false;

    }

    void Ready()
    {
        if (!status.isSkillReady)
        {
            status.isSkillReady = true;
        }
        else if (status.isSkillReady)
        {
            status.isSkillReady = false;
            skillList[stats.skill[status.skillIndex]].skillCoolTime = 0.5f;
        }
    }

    public IEnumerator Hold()
    {
        status.isSkillHold = true;
        //player.RunDelay();

        skillList[stats.skill[status.skillIndex]].Use(gameObject);

        float timer = skillList[stats.skill[status.skillIndex]].rate;

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            timer -= 0.1f;
            if (timer <= 0)
            {
                HoldOut();
                break;
            }
        }

    }

    public void HoldOut()
    {
        skillList[stats.skill[status.skillIndex]].Exit(gameObject);
        status.isSkillHold = false;
    }

}
