using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats stats;

    // ��ų ����Ʈ
    [SerializeField] GameObject[] skillList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        stats = GetComponent<PlayerStats>();
    }

    // ��ų ȹ��
    public void EquipSkill(Skill gainSkill)
    {
        stats.skill = skillList[gainSkill.skillID].GetComponent<Skill>();
        Destroy(gainSkill.gameObject);
        MapUIManager.instance.UpdateSkillUI();

    }

    // ��ų ����
    public void UnEquipSkill()
    {
        Instantiate(DataManager.instance.gameData.skillList[stats.skill.skillID], gameObject.transform.position, gameObject.transform.localRotation);
        stats.skill = null;
        MapUIManager.instance.UpdateSkillUI();
    }

    // ��ųŰ �Է�
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
            StartCoroutine(Hold());
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
            yield return new WaitForSeconds(skillRate / stats.attackSpeed * stats.weapon.attackSpeed);
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
            stats.skill.skillCoolTime = 0.5f;
        }
    }

    public IEnumerator Hold()
    {
        status.isSkillHold = true;
        //player.RunDelay();

        stats.skill.Use(gameObject);

        float timer = stats.skill.rate;

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
        stats.skill.Exit(gameObject);
        status.isSkillHold = false;
    }

}
