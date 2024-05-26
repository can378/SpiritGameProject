using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceEnvy : EnemyBasic
{
    //질투=스킬모방(쥐 처럼 대신 강도는 더 강하게)

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

        if (!enemyTarget)
        {
            RandomMove();
        }
        else if (targetDis < 6f)
        {
            Run();
        }
        else if (targetDis >= 7f)
        {
            Chase();
        }
        else
        {
            moveVec = Vector3.zero;
        }
    }

    protected override void AttackPattern()
    {
        // 스킬 사용이 가능하면 스킬 사용
        if (skill != 0 && skillList[skill].skillCoolTime <= 0)
        {
            StartCoroutine(Skill());
        }
    }

    IEnumerator Skill()
    {
        print("Skill");

        isAttack = true;
        isAttackReady = false;

        //mimic player skill
        print("mimic player skill");

        yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].preDelay : 0);

        skillList[skill].Enter(gameObject);

        yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].postDelay : 0);

        yield return new WaitForSeconds(skillList[skill].skillType != 0 ? skillList[skill].maxHoldTime / 2 : 0);

        yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].preDelay : 0);

        skillList[skill].Exit();

        yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].postDelay : 0);

        isAttack = false;
        isAttackReady = true;

    }

    void Change()
    {
        if (!hitTarget || isFlinch)
            return;

        if (hitTarget.tag == "Player")
        {
            skill = hitTarget.GetComponent<Player>().playerStats.skill[hitTarget.GetComponent<Player>().status.skillIndex];
            if (skill != 0) skillList[skill].gameObject.SetActive(true);
        }
    }
}
