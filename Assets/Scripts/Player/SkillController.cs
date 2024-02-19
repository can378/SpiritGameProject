using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    Player player;

    // 얻은 무기 정보
    public Skill skill;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    // 무기를 획득
    public void EquipSkill(Skill gainSkill)
    {
        skill = gainSkill;
        //skill.gameObject.SetActive(false);
    }

    public void UnEquipSkill()
    {
        skill.gameObject.transform.position = gameObject.transform.position;
        //skill.gameObject.SetActive(true);
        skill = null;
    }

    public void SkillDown()
    {
        if (skill.skillType == 0)
        {
            // 즉발
            Debug.Log("스킬 즉시 시전");
            StartCoroutine("Immediate");
        }
        else if (skill.skillType == 1)
        {
            //준비
            Debug.Log("스킬 준비");
            StartCoroutine("Ready");
        }
        else if (skill.skillType == 2)
        {
            //홀드
            Debug.Log("스킬 홀드");
            StartCoroutine("Hold");
        }

    }

    public IEnumerator Immediate()
    {
        Debug.Log("스킬 시전");
        player.status.isSkillReady = false;
        player.status.isSkill = true;
        player.RunDelay();

        skill.Use(gameObject);

        float skillRate = skill.preDelay + skill.rate + skill.postDelay;
        if (skill.skillLimit == SkillLimit.None)
        {
            yield return new WaitForSeconds(skillRate / player.userData.playerAttackSpeed);
        }
        else
        {
            yield return new WaitForSeconds(skillRate / player.userData.playerAttackSpeed * player.mainWeaponController.mainWeapon.attackSpeed);
        }

        player.status.isSkill = false;

    }

    void Ready()
    {
        if(!player.status.isSkillReady)
        {
            player.status.isSkillReady = true;
        }
        else if(player.status.isSkillReady)
        {
            player.status.isSkillReady = false;
            skill.skillCoolTime = 0.5f;
        }
    }

    IEnumerator Hold()
    {
        player.status.isSkillHold = true;
        player.RunDelay();

        if (skill.skillLimit == SkillLimit.None)
        {
            yield return new WaitForSeconds(skill.preDelay / player.userData.playerAttackSpeed);
        }
        else
        {
            yield return new WaitForSeconds(skill.preDelay / player.userData.playerAttackSpeed * player.mainWeaponController.mainWeapon.attackSpeed);
        }

        skill.Use(gameObject);

        while(player.status.isSkillHold)
        {
            if(player.skUp)
            {
                skill.Exit(gameObject);
                if (skill.skillLimit == SkillLimit.None)
                {
                    yield return new WaitForSeconds(skill.postDelay / player.userData.playerAttackSpeed);
                }
                else
                {
                    yield return new WaitForSeconds(skill.postDelay / player.userData.playerAttackSpeed * player.mainWeaponController.mainWeapon.attackSpeed);
                }
                player.status.isSkillHold = false;
            }
        }

    }

}
