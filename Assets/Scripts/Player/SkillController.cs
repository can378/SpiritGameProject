using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats stats;

    Coroutine skillCoroutine;

    // 스킬 리스트
    public Skill[] skillList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        stats = GetComponent<PlayerStats>();
    }

    // 스킬 획득
    public bool EquipSkill(int skillID)
    {
        // 이미 보유한 스킬이라면
        if(skillList[skillID].gameObject.activeSelf == true)
            return false;
            
        stats.skill[status.skillIndex] = skillID;
        skillList[skillID].gameObject.SetActive(true);
        return true;
    }

    // 스킬 해제
    public void UnEquipSkill()
    {
        Instantiate(DataManager.instance.gameData.skillList[stats.skill[status.skillIndex]], gameObject.transform.position, gameObject.transform.localRotation);
        skillList[stats.skill[status.skillIndex]].gameObject.SetActive(false);
        stats.skill[status.skillIndex] = 0;
    }

    // 스킬키 입력
    public void SkillDown()
    {
        skillCoroutine = StartCoroutine(Enter());
    }

    public IEnumerator Enter()
    {
        // 홀드 중
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
