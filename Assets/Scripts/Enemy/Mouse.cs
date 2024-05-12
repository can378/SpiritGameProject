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
            isChase = true;
        }
        else
        {
            if (targetDis < 6f)
            {
                isRun = true;
                isChase = false;
            }
            else if (targetDis >= 7f)
            {
                isChase = true;
                isRun = false;
            }
            else
            {
                isChase = false;
                isRun = false;
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
        print("Bite");

        isAttack = true;
        isAttackReady = false;

        // 물기 전 대기 시간
        yield return new WaitForSeconds(0.5f);

        HitDetection hitDetection = biteArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;

        biteArea.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(enemyTarget.transform.position.y - transform.position.y, enemyTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        biteArea.SetActive(true);

        // 물기 판정 유지 시간
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
