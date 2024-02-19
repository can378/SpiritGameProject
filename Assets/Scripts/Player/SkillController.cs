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

    public void Use()
    {
        if (skill.skillType == 0)
        {
            // ���
            StartCoroutine("Immediate");
        }
        else if (skill.skillType == 1)
        {
            //�غ�
            StartCoroutine("Ready");
        }
        else if (skill.skillType == 2)
        {
            //Ȧ��
            StartCoroutine("Hold");
        }

    }

    IEnumerator Immediate()
    {
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

    IEnumerator Ready()
    {
        player.status.isSkillReady = true;
        player.RunDelay();

        while(player.status.isSkillReady)
        {
            // ��ų ���
            if(player.skDown)
            {
                skill.skillCoolTime = 0.5f;
                //��ų ���� ���ð� �߰�
            }

            // ��ų ���
            if(player.sDown)
            {
                player.status.isSkillReady = false;
                player.status.isSkill = true;

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
