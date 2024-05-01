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
    public bool EquipSkill(int skillID)
    {
        // �̹� ������ ��ų�̶��
        if(skillList[skillID].gameObject.activeSelf == true)
            return false;
            
        stats.skill[status.skillIndex] = skillID;
        skillList[skillID].gameObject.SetActive(true);
        MapUIManager.instance.UpdateSkillUI();
        return true;
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

        if(skillList[stats.skill[status.skillIndex]].skillType == 0)
        {
            status.isSkill = true;
            yield return new WaitForSeconds(skillList[stats.skill[status.skillIndex]].preDelay);
        }

        skillList[stats.skill[status.skillIndex]].Enter(gameObject);

        if (skillList[stats.skill[status.skillIndex]].skillType == 0)
        {
            yield return new WaitForSeconds(skillList[stats.skill[status.skillIndex]].postDelay);
            status.isSkill = false;
        }

        StartCoroutine(Stay());
    }

    public IEnumerator Stay()
    {
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
        
        if (skillList[stats.skill[status.skillIndex]].skillType == 2)
        {
            status.isSkill = true;
            yield return new WaitForSeconds(skillList[stats.skill[status.skillIndex]].preDelay);
        }

        skillList[stats.skill[status.skillIndex]].Exit();

        if (skillList[stats.skill[status.skillIndex]].skillType == 2)
        {
            yield return new WaitForSeconds(skillList[stats.skill[status.skillIndex]].postDelay);
            status.isSkill = false;
        }

        


    }

}
