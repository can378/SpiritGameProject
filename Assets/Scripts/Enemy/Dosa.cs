using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dosa : EnemyBasic
{
    [field: SerializeField] public List<Skill> skillList { get; private set; }

    int skill;


    protected override void Start()
    {
        base.Start();
        skill=UnityEngine.Random.Range(1,skillList.Count);
        skillList[skill].gameObject.SetActive(true);
    }

    protected override void AttackPattern()
    {
        //Debug.Log("skill num="+skill);
        //스킬 사용
        if (skill != 0 && skillList[skill].skillCoolTime <= 0)
        {
            Debug.Log("start skill");
            enemyStatus.attackCoroutine = StartCoroutine(Skill());
        }
    }


    IEnumerator Skill()
    {
        print("Dosa Skill");
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].preDelay : 0);

        skillList[skill].Enter(gameObject);

        yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].postDelay : 0);

        yield return new WaitForSeconds(skillList[skill].skillType != 0 ? skillList[skill].maxHoldTime / 2 : 0);

        yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].preDelay : 0);

        skillList[skill].Exit();
        enemyStatus.attackCoroutine = null;

        yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].postDelay : 0);


        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;

    }

    public override void AttackCancle()
    {
        status.isAttack = false;
        status.isAttackReady = true;
        status.moveVec = Vector2.zero;

        foreach(GameObject hitEffect in hitEffects)
            hitEffect.SetActive(false);

        if(enemyStatus.attackCoroutine != null)
        {
            StopCoroutine(enemyStatus.attackCoroutine);
            skillList[skill].Cancle();
            enemyStatus.attackCoroutine = null;
        }

    }

}
