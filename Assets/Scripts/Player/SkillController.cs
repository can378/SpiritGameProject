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
        stats.skill = skillID;
        skillList[skillID].gameObject.SetActive(true);
        Destroy(gameObject);
        MapUIManager.instance.UpdateSkillUI();

    }

    // 스킬 해제
    public void UnEquipSkill()
    {
        Instantiate(DataManager.instance.gameData.skillList[stats.skill], gameObject.transform.position, gameObject.transform.localRotation);
        skillList[stats.skill].gameObject.SetActive(false);
        stats.skill = 0;
        MapUIManager.instance.UpdateSkillUI();
    }

    // 스킬키 입력
    public void SkillDown()
    {
        if (skillList[stats.skill].skillType == 0)
        {
            // 즉발
            Debug.Log("스킬 즉시 시전");
            StartCoroutine("Immediate");
        }
        else if (skillList[stats.skill].skillType == 1)
        {
            //준비
            Debug.Log("스킬 준비");
            StartCoroutine("Ready");
        }
        else if (skillList[stats.skill].skillType == 2)
        {
            //홀드
            Debug.Log("스킬 홀드");
            StartCoroutine(Hold());
        }

    }

    public IEnumerator Immediate()
    {
        Debug.Log("스킬 시전");
        status.isSkillReady = false;
        status.isSkill = true;

        skillList[stats.skill].Use(gameObject);


        // 스킬 시전 시간 (다음 움직이기 까지 대기 시간)
        float skillUsedTime = skillList[stats.skill].preDelay + skillList[stats.skill].rate + skillList[stats.skill].postDelay;

        if (skillList[stats.skill].skillLimit == SkillLimit.None)
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
            skillList[stats.skill].skillCoolTime = 0.5f;
        }
    }

    public IEnumerator Hold()
    {
        status.isSkillHold = true;
        //player.RunDelay();

        skillList[stats.skill].Use(gameObject);

        float timer = skillList[stats.skill].rate;

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
        skillList[stats.skill].Exit(gameObject);
        status.isSkillHold = false;
    }

}
