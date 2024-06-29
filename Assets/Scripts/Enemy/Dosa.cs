using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dosa : EnemyBasic
{
    [field: SerializeField] public List<Skill> skillList { get; private set; }
    public GameObject biteArea;


    private int skill;
    

    private void Start()
    {
        base.Start();
        skill=UnityEngine.Random.Range(0,skillList.Count);
    }
    protected override void AttackPattern()
    {
        //스킬 사용
        if (skill != 0 && skillList[skill].skillCoolTime <= 0)
        {
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
