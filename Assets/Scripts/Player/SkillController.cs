using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats stats;

    // 스킬 리스트
    public Skill[] skillList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        stats = GetComponent<PlayerStats>();
    }

    // 스킬 획득
    public void EquipSkill(int skillID)
    {
        stats.skill[status.skillIndex] = skillID;
        skillList[skillID].gameObject.SetActive(true);
        MapUIManager.instance.UpdateSkillUI();

    }

    // 스킬 해제
    public void UnEquipSkill()
    {
        Instantiate(DataManager.instance.gameData.skillList[stats.skill[status.skillIndex]], gameObject.transform.position, gameObject.transform.localRotation);
        skillList[stats.skill[status.skillIndex]].gameObject.SetActive(false);
        stats.skill[status.skillIndex] = 0;
        MapUIManager.instance.UpdateSkillUI();
    }

    // 스킬키 입력
    public void SkillDown()
    {
        if (skillList[stats.skill[status.skillIndex]].skillType == 0)
        {
            // 즉발
            Debug.Log("스킬 시전");
            StartCoroutine(Action());
        }
        else if (skillList[stats.skill[status.skillIndex]].skillType == 1)
        {
            //홀드
            Debug.Log("스킬 홀드");
            StartCoroutine(Hold());
        }

    }

    public IEnumerator Action()
    {
        status.isSkill = true;

        skillList[stats.skill[status.skillIndex]].Use(gameObject);

        // 스킬 시전 시간 (다음 움직이기 까지 대기 시간)
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
