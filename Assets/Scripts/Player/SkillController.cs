using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats stats;
    
    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        stats = GetComponent<PlayerStats>();
    }

    // ��ų ȹ��
    public void EquipSkill(Skill gainSkill)
    {
        stats.skill = gainSkill;
        MapUIManager.instance.UpdateSkillUI();
        //skill.gameObject.SetActive(false);
    }

    public void UnEquipSkill()
    {
        stats.skill.gameObject.transform.position = gameObject.transform.position;
        //skill.gameObject.SetActive(true);
        stats.skill = null;
        MapUIManager.instance.UpdateSkillUI();
    }

    public void SkillDown()
    {
        if (stats.skill.skillType == 0)
        {
            // ���
            Debug.Log("��ų ��� ����");
            StartCoroutine("Immediate");
        }
        else if (stats.skill.skillType == 1)
        {
            //�غ�
            Debug.Log("��ų �غ�");
            StartCoroutine("Ready");
        }
        else if (stats.skill.skillType == 2)
        {
            //Ȧ��
            Debug.Log("��ų Ȧ��");
            Hold();
        }

    }

    public IEnumerator Immediate()
    {
        Debug.Log("��ų ����");
        status.isSkillReady = false;
        status.isSkill = true;

        stats.skill.Use(gameObject);


        // ��ų ���� �ð� (���� �����̱� ���� ��� �ð�)
        float skillRate = stats.skill.preDelay + stats.skill.rate + stats.skill.postDelay;
        if (stats.skill.skillLimit == SkillLimit.None)
        {
            yield return new WaitForSeconds(skillRate / stats.attackSpeed);
        }
        else
        {
            yield return new WaitForSeconds(skillRate / stats.attackSpeed * stats.mainWeapon.attackSpeed);
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
            stats.skill.skillCoolTime = 0.5f;
        }
    }

    void Hold()
    {
        status.isSkillHold = true;
        //player.RunDelay();

        stats.skill.Use(gameObject);

    }

    public void HoldOut()
    {
        stats.skill.Exit(gameObject);
        status.isSkillHold = false;
    }

}
