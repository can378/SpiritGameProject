using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    Player player;

    // ���� ���� ����
    public Skill skill;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    // ���⸦ ȹ��
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
            // ���
            Debug.Log("��ų ��� ����");
            StartCoroutine("Immediate");
        }
        else if (skill.skillType == 1)
        {
            //�غ�
            Debug.Log("��ų �غ�");
            StartCoroutine("Ready");
        }
        else if (skill.skillType == 2)
        {
            //Ȧ��
            Debug.Log("��ų Ȧ��");
            StartCoroutine("Hold");
        }

    }

    public IEnumerator Immediate()
    {
        Debug.Log("��ų ����");
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
