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
        StartCoroutine(Enter());
    }

    public IEnumerator Enter()
    {
        // 홀드 중
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

        // 선딜
        status.isSkill = true;

        yield return new WaitForSeconds(skillList[stats.skill[status.skillIndex]].preDelay / stats.attackSpeed);

        // 사용
        skillList[stats.skill[status.skillIndex]].Exit(gameObject);

        // 후딜

        yield return new WaitForSeconds(skillList[stats.skill[status.skillIndex]].postDelay / stats.attackSpeed);

        status.isSkill = false;

        
        
    }

}
