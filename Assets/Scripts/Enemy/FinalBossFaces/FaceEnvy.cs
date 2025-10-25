using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceEnvy : BossFace
{
    //ï¿½ï¿½ï¿½ï¿½=ï¿½ï¿½Å³ï¿½ï¿½ï¿?(ï¿½ï¿½ Ã³ï¿½ï¿½ ï¿½ï¿½ï¿? ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½Ï°ï¿½)

    [field: SerializeField] public List<SkillBase> skillList { get; private set; }
    [field: SerializeField] public int skill { get; private set; }
    public GameObject biteArea;


    protected override void Update()
    {
        base.Update();
        Change();
    }

    protected override void MovePattern()
    {
        if (nowAttack)
        {
            if (!enemyStatus.isTarget)
            {
                RandomMove();
            }
            else if (enemyStatus.targetDis < 6f)
            {
                Run();
            }
            else if (enemyStatus.targetDis >= 7f)
            {
                Chase();
            }
            else
            {
                enemyStatus.moveVec = Vector3.zero;
            }
        }
    }

    protected override void init()
    {
        base.init();
        skill = UnityEngine.Random.Range(1, skillList.Count);
        if(skill==9) { skill = 1; }
        skillList[skill].gameObject.SetActive(true);
    }

    protected override void faceAttack()
    {
        if (skill != 0 && skillList[skill].skillCoolTime <= 0)
        {
            StartCoroutine(Skill());
        }
    }


    IEnumerator Skill()
    {
        print("envy Skill");

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        //mimic player skill
        print("mimic player skill");

        yield return new WaitForSeconds(skillList[skill].skillData.skillType == SKILL_TYPE.DOWN ? skillList[skill].skillData.preDelay : 0);

        skillList[skill].Enter(this);

        yield return new WaitForSeconds(skillList[skill].skillData.skillType == SKILL_TYPE.DOWN ? skillList[skill].skillData.postDelay : 0);

        yield return new WaitForSeconds(skillList[skill].skillData.skillType != SKILL_TYPE.DOWN ? skillList[skill].skillData.maxHoldTime / 2 : 0);

        yield return new WaitForSeconds(skillList[skill].skillData.skillType == SKILL_TYPE.UP ? skillList[skill].skillData.preDelay : 0);

        skillList[skill].Exit();

        yield return new WaitForSeconds(skillList[skill].skillData.skillType == SKILL_TYPE.UP ? skillList[skill].skillData.postDelay : 0);

        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;

    }

    void Change()
    {
        if (!enemyStatus.hitTarget || (0 < enemyStatus.isFlinch))
            return;

        if (enemyStatus.hitTarget.tag == "Player")
        {
            int playerSkill = enemyStatus.hitTarget.GetComponent<Player>().playerStats.skill[enemyStatus.hitTarget.GetComponent<Player>().playerStatus.skillIndex].itemData.selectItemID;
            if (playerSkill != 0 || playerSkill!=9) { skill = playerSkill; }

            print("envy");
            skillList[skill].gameObject.SetActive(true);

            //if (skill != 0) { skillList[skill].gameObject.SetActive(true); }

        }
    }

    public override void FlinchCancle()
    {
        status.isAttack = false;
        status.isAttackReady = true;
        status.moveVec = Vector2.zero;

        foreach (GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);

        if (enemyStatus.attackCoroutine != null)
        {
            StopCoroutine(enemyStatus.attackCoroutine);
            skillList[skill].Cancle();
            enemyStatus.attackCoroutine = null;
        }

    }


}
