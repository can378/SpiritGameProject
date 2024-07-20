using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dosa : EnemyBasic
{
    [field: SerializeField] public List<Skill> skillList { get; private set; }


    private int skill;


    protected override void Start()
    {
        base.Start();
        skill=UnityEngine.Random.Range(1,skillList.Count);
        skillList[skill].gameObject.SetActive(true);
    }
    protected override void AttackPattern()
    {
        Debug.Log("skill num="+skill);
        //스킬 사용
        if (skill != 0 && skillList[skill].skillCoolTime <= 0)
        {
            Debug.Log("start skill");
            StartCoroutine(Skill());
        }
    }


    IEnumerator Skill()
    {
        print("Dosa Skill");
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

}
