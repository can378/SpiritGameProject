using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dosa : EnemyBasic
{
    [field: SerializeField] public List<Skill> skillList { get; private set; }

    public int skill ;


    protected override void Start()
    {
        base.Start();

        // 기술 무작위 장착
        skill = -1;
        while (true)
        {
            skill = UnityEngine.Random.Range(1, skillList.Count);
            if (skillList[skill] == null)
            {
                skill = -1;
                continue;
            }
            else
            {
                skillList[skill].gameObject.SetActive(true);
                break;
            }
        }

    }

    protected override void AttackPattern()
    {
        //Debug.Log("skill num="+skill);
        //스킬 사용
        if (skill != 0 && skillList[skill].skillCoolTime <= 0)
        {
            enemyStatus.attackCoroutine = StartCoroutine(Skill());
        }
    }


    IEnumerator Skill()
    {
        SKILL_TYPE _Type = (SKILL_TYPE)skillList[skill].skillType;

        // 스킬 루틴 시작
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;


        if(_Type == SKILL_TYPE.DOWN)
        {
            // 애니메이션 시작
            enemyAnim.animator.SetBool("isAttack", true);

            // Down 스킬 : 시전 선딜
            yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].preDelay : 0);

            // 스킬 시작
            skillList[skill].Enter(this);

            // Down 스킬 : 시전 후딜
            yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].postDelay : 0);

            // 스킬 끝
            skillList[skill].Exit();
            enemyStatus.attackCoroutine = null;

            // 애니메이션 끝
            enemyAnim.animator.SetBool("isAttack", false);
        }
        else if(_Type == SKILL_TYPE.HOLD)
        {
            // 애니메이션 시작
            enemyAnim.animator.SetBool("isAttack", true);

            // 스킬 시작
            skillList[skill].Enter(this);

            // Hold 스킬 : 스킬 유지되는 시간
            yield return new WaitForSeconds(skillList[skill].skillType != 0 ? skillList[skill].maxHoldTime / 2 : 0);

            // 종료
            skillList[skill].Exit();
            enemyStatus.attackCoroutine = null;

            // 애니메이션 끝
            enemyAnim.animator.SetBool("isAttack", false);
        }
        else if(_Type == SKILL_TYPE.UP)
        {
            // 스킬 시작
            skillList[skill].Enter(this);

            // Up 스킬 : 키 Up 전 대기 시간 
            yield return new WaitForSeconds(skillList[skill].skillType != 0 ? skillList[skill].maxHoldTime / 2 : 0);

            // 애니메이션 시작
            enemyAnim.animator.SetBool("isAttack", true);

            // Up 스킬 : 시전 선딜
            yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].preDelay : 0);

            // 스킬 종료
            skillList[skill].Exit();
            enemyStatus.attackCoroutine = null;

            // Up 스킬 : 시전 후딜
            yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].postDelay : 0);

            // 애니메이션 끝
            enemyAnim.animator.SetBool("isAttack", false);
        }


        // 스킬 루틴 끝
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
