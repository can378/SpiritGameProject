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
            Debug.Log("��ų ����");
            StartCoroutine(Action());
        }
        else if (skillList[stats.skill[status.skillIndex]].skillType == 1)
        {
            //Ȧ��
            Debug.Log("��ų Ȧ��");
            StartCoroutine(Hold());
        }

    }

    public IEnumerator Action()
    {
        status.isSkill = true;

        skillList[stats.skill[status.skillIndex]].Use(gameObject);

        // ��ų ���� �ð� (���� �����̱� ���� ��� �ð�)
        float skillUsedTime = skillList[stats.skill[status.skillIndex]].preDelay + skillList[stats.skill[status.skillIndex]].rate + skillList[stats.skill[status.skillIndex]].postDelay;

        yield return new WaitForSeconds(skillUsedTime / stats.attackSpeed);

        status.isSkill = false;

    }

    public IEnumerator Hold()
    {
        status.isSkillHold = true;

        skillList[stats.skill[status.skillIndex]].Use(gameObject);

        float timer = skillList[stats.skill[status.skillIndex]].maxHold;

        while (status.isSkillHold)
        {
            yield return new WaitForSeconds(0.1f);
            timer -= 0.1f;
            if (timer <= 0)
            {
                StartCoroutine(HoldOut());
                break;
            }
        }

    }

    public IEnumerator HoldOut()
    {
        skillList[stats.skill[status.skillIndex]].Exit(gameObject);
        status.isSkillHold = false;

        status.isSkill = true;

        float skillUsedTime = skillList[stats.skill[status.skillIndex]].postDelay;

        yield return new WaitForSeconds(skillUsedTime / stats.attackSpeed);

        status.isSkill = false;

        
        
    }

}
