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
            if(!enemyTarget)
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
    }

    protected override void AttackPattern()
    {
        // 가까이 있으면 깨문다.
        if(targetDis < 2f)
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
        Vector3 hitDir = targetDirVec;

        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        hitDetection = biteArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
        hitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 10, 0, 0, null);
        biteArea.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90);
        biteArea.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        biteArea.SetActive(false);
        isAttack = false;
        isAttackReady = true;
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
        if(isChange || !hitTarget || isFlinch)
            return;

        if (hitTarget.tag == "Player")
        {
            GetComponentInChildren<SpriteRenderer>().sprite = hitTarget.GetComponentInChildren<SpriteRenderer>().sprite;
            GetComponentInChildren<SpriteRenderer>().transform.localScale = hitTarget.GetComponentInChildren<SpriteRenderer>().transform.localScale;
            isChange = true;

            skill = hitTarget.GetComponent<Player>().playerStats.skill[hitTarget.GetComponent<Player>().status.skillIndex];
            if (skill != 0) skillList[skill].gameObject.SetActive(true);

            //Run away
            targetDirVec = hitTarget.transform.position - transform.position;
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 10);
        }
    }

}
