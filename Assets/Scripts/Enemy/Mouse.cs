using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mouse : EnemyBasic
{
    [field : SerializeField] public List<Skill> skillList {get; private set;}
    [field: SerializeField] public int skill {get; private set;}
    public GameObject biteArea;
    private bool isChange = false;

    protected override void Update()
    {
        base.Update();
        Change();
    }

    protected override void MovePattern()
    {
        if (!isChange)
        {
            if(!enemyStatus.enemyTarget)
            {
                RandomMove();
            }
            else
            {
                Chase();
            }
        }
        else
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

    protected override void AttackPattern()
    {
        // 가까이 있으면 깨문다.
        if(enemyStatus.targetDis < 2f)
        {
            StartCoroutine(Bite());
            return;
        }

        // 변신 상태라면
        if (isChange)
        {
            // 스킬 사용이 가능하면 스킬 사용
            if (skill != 0 && skillList[skill].skillCoolTime <= 0)
            {
                StartCoroutine(Skill());
            }
        }
    }

    IEnumerator Bite()
    {
        HitDetection hitDetection;
        Vector3 hitDir = enemyStatus.targetDirVec;

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        hitDetection = biteArea.GetComponent<HitDetection>();
        hitDetection.user = this;
        hitDetection.SetHit_Ratio(1, 1, enemyStats.AttackPower);
        biteArea.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90);
        biteArea.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        biteArea.SetActive(false);
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }

    IEnumerator Skill()
    {
        print("Skill");

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        //mimic player skill
        print("mimic player skill");

        yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].preDelay : 0);

        skillList[skill].Enter(this);

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
        if(isChange || !enemyStatus.hitTarget || (0 < enemyStatus.isFlinch))
            return;

        if (enemyStatus.hitTarget.tag == "Player")
        {
            GetComponentInChildren<SpriteRenderer>().sprite = enemyStatus.hitTarget.GetComponentInChildren<SpriteRenderer>().sprite;
            GetComponentInChildren<SpriteRenderer>().transform.localScale = enemyStatus.hitTarget.GetComponentInChildren<SpriteRenderer>().transform.localScale;
            isChange = true;

            skill = enemyStatus.hitTarget.GetComponent<Player>().playerStats.skill[enemyStatus.hitTarget.GetComponent<Player>().playerStatus.skillIndex];
            if (skill != 0) skillList[skill].gameObject.SetActive(true);

            //Run away
            enemyStatus.targetDirVec = enemyStatus.hitTarget.transform.position - transform.position;
            rigid.AddForce(-enemyStatus.targetDirVec * GetComponent<EnemyStats>().MoveSpeed.Value * 10);
        }
    }

}
