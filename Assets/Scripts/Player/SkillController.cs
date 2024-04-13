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
        StartCoroutine(Enter());
    }

    public IEnumerator Enter()
    {
        // Ȧ�� ��
        status.isSkillHold = true;

        skillList[stats.skill[status.skillIndex]].Enter(gameObject);

        float timer = skillList[stats.skill[status.skillIndex]].maxHoldTime;

        while (status.isSkillHold)
        {
            yield return new WaitForSeconds(0.1f);
            timer -= 0.1f;
            if (timer <= 0)
            {
                StartCoroutine(Exit());
                break;
            }
        }

    }

    public IEnumerator Exit()
    {
        status.isSkillHold = false;

        // ����
        status.isSkill = true;

        yield return new WaitForSeconds(skillList[stats.skill[status.skillIndex]].preDelay / stats.attackSpeed);

        // ���
        skillList[stats.skill[status.skillIndex]].Exit(gameObject);

        // �ĵ�

        yield return new WaitForSeconds(skillList[stats.skill[status.skillIndex]].postDelay / stats.attackSpeed);

        status.isSkill = false;

        
        
    }

}
