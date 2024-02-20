using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [field: SerializeField] public Skill skill { get; set; }                        // ��ų
    PlayerStatus status;
    
    void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    // ��ų ȹ��
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
        status.isSkillReady = false;
        status.isSkill = true;

        skill.Use(gameObject);

        float skillRate = skill.preDelay + skill.rate + skill.postDelay;
        if (skill.skillLimit == SkillLimit.None)
        {
            yield return new WaitForSeconds(skillRate / DataManager.instance.userData.playerAttackSpeed);
        }
        else
        {
            yield return new WaitForSeconds(skillRate / DataManager.instance.userData.playerAttackSpeed * Player.instance.mainWeaponController.mainWeapon.attackSpeed);
        }

        status.isSkill = false;

    }

    void Ready()
    {
        if(!status.isSkillReady)
        {
            status.isSkillReady = true;
        }
        else if(status.isSkillReady)
        {
            status.isSkillReady = false;
            skill.skillCoolTime = 0.5f;
        }
    }

    IEnumerator Hold()
    {
        status.isSkillHold = true;
        //player.RunDelay();

        if (skill.skillLimit == SkillLimit.None)
        {
            yield return new WaitForSeconds(skill.preDelay / DataManager.instance.userData.playerAttackSpeed);
        }
        else
        {
            yield return new WaitForSeconds(skill.preDelay / DataManager.instance.userData.playerAttackSpeed * Player.instance.mainWeaponController.mainWeapon.attackSpeed);
        }

        skill.Use(gameObject);

    }

    IEnumerator HoldOut()
    {
        skill.Exit(gameObject);
        if (skill.skillLimit == SkillLimit.None)
        {
            yield return new WaitForSeconds(skill.postDelay / DataManager.instance.userData.playerAttackSpeed);
        }
        else
        {
            yield return new WaitForSeconds(skill.postDelay / DataManager.instance.userData.playerAttackSpeed * Player.instance.mainWeaponController.mainWeapon.attackSpeed);
        }
        status.isSkillHold = false;
    }

}
