using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats stats;

    Coroutine skillCoroutine;

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
        return true;
    }

    // ��ų ����
    public void UnEquipSkill()
    {
        Instantiate(DataManager.instance.gameData.skillList[stats.skill[status.skillIndex]], gameObject.transform.position, gameObject.transform.localRotation);
        skillList[stats.skill[status.skillIndex]].gameObject.SetActive(false);
        stats.skill[status.skillIndex] = 0;
    }

    // ��ųŰ �Է�
    public void SkillDown()
    {
        skillCoroutine = StartCoroutine(Enter());
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

        skillCoroutine = StartCoroutine(Stay());
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
                skillCoroutine = StartCoroutine(Exit());
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

        skillCoroutine = null;

        if (skillList[stats.skill[status.skillIndex]].skillType == 2)
        {
            yield return new WaitForSeconds(skillList[stats.skill[status.skillIndex]].postDelay);
            status.isSkill = false;
        }
    }

    public void SkillCancle()
    {
        status.isSkillHold = false;
        status.isSkill = false;
        if (skillCoroutine != null) StopCoroutine(skillCoroutine);
        if(stats.skill[status.skillIndex] == 0) skillList[stats.skill[status.skillIndex]].Cancle();
    }

}
