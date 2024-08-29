using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceEnvy : BossFace
{
    //����=��ų���(�� ó�� ��� ������ �� ���ϰ�)

    [field: SerializeField] public List<Skill> skillList { get; private set; }
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
            if (!enemyStatus.enemyTarget)
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

        yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].preDelay : 0);

        skillList[skill].Enter(gameObject);

        yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].postDelay : 0);

        yield return new WaitForSeconds(skillList[skill].skillType != 0 ? skillList[skill].maxHoldTime / 2 : 0);

        yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].preDelay : 0);

        skillList[skill].Exit();

        yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].postDelay : 0);

        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;

    }

    void Change()
    {
        if (!enemyStatus.hitTarget || enemyStatus.isFlinch)
            return;

        if (enemyStatus.hitTarget.tag == "Player")
        {
            skill = enemyStatus.hitTarget.GetComponent<Player>().playerStats.skill[enemyStatus.hitTarget.GetComponent<Player>().playerStatus.skillIndex];
            if (skill != 0) skillList[skill].gameObject.SetActive(true);
        }
    }
}
