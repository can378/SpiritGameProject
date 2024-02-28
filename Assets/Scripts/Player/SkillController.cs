using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [field: SerializeField] public Skill skill { get; set; }                        // 스킬
    PlayerStatus status;
    
    void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    // 스킬 획득
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
            Hold();
        }

    }

    public IEnumerator Immediate()
    {
        Debug.Log("스킬 시전");
        status.isSkillReady = false;
        status.isSkill = true;

        skill.Use(gameObject);


        // 스킬 시전 시간 (다음 움직이기 까지 대기 시간)
        float skillRate = skill.preDelay + skill.rate + skill.postDelay;
        if (skill.skillLimit == SkillLimit.None)
        {
            yield return new WaitForSeconds(skillRate / Player.instance.userData.playerAttackSpeed);
        }
        else
        {
            yield return new WaitForSeconds(skillRate / Player.instance.userData.playerAttackSpeed * Player.instance.mainWeaponController.mainWeapon.attackSpeed);
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

    void Hold()
    {
        status.isSkillHold = true;
        //player.RunDelay();

        skill.Use(gameObject);

    }

    public void HoldOut()
    {
        skill.Exit(gameObject);
        status.isSkillHold = false;
    }

}
