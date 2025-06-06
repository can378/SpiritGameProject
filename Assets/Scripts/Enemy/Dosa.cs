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

        // ��� ������ ����
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
        //��ų ���
        if (skill != 0 && skillList[skill].skillCoolTime <= 0)
        {
            enemyStatus.attackCoroutine = StartCoroutine(Skill());
        }
    }


    IEnumerator Skill()
    {
        SKILL_TYPE _Type = (SKILL_TYPE)skillList[skill].skillType;

        // ��ų ��ƾ ����
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;


        if(_Type == SKILL_TYPE.DOWN)
        {
            // �ִϸ��̼� ����
            enemyAnim.animator.SetBool("isAttack", true);

            // Down ��ų : ���� ����
            yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].preDelay : 0);

            // ��ų ����
            skillList[skill].Enter(this);

            // Down ��ų : ���� �ĵ�
            yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].postDelay : 0);

            // ��ų ��
            skillList[skill].Exit();
            enemyStatus.attackCoroutine = null;

            // �ִϸ��̼� ��
            enemyAnim.animator.SetBool("isAttack", false);
        }
        else if(_Type == SKILL_TYPE.HOLD)
        {
            // �ִϸ��̼� ����
            enemyAnim.animator.SetBool("isAttack", true);

            // ��ų ����
            skillList[skill].Enter(this);

            // Hold ��ų : ��ų �����Ǵ� �ð�
            yield return new WaitForSeconds(skillList[skill].skillType != 0 ? skillList[skill].maxHoldTime / 2 : 0);

            // ����
            skillList[skill].Exit();
            enemyStatus.attackCoroutine = null;

            // �ִϸ��̼� ��
            enemyAnim.animator.SetBool("isAttack", false);
        }
        else if(_Type == SKILL_TYPE.UP)
        {
            // ��ų ����
            skillList[skill].Enter(this);

            // Up ��ų : Ű Up �� ��� �ð� 
            yield return new WaitForSeconds(skillList[skill].skillType != 0 ? skillList[skill].maxHoldTime / 2 : 0);

            // �ִϸ��̼� ����
            enemyAnim.animator.SetBool("isAttack", true);

            // Up ��ų : ���� ����
            yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].preDelay : 0);

            // ��ų ����
            skillList[skill].Exit();
            enemyStatus.attackCoroutine = null;

            // Up ��ų : ���� �ĵ�
            yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].postDelay : 0);

            // �ִϸ��̼� ��
            enemyAnim.animator.SetBool("isAttack", false);
        }


        // ��ų ��ƾ ��
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
