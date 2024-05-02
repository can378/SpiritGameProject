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


    private void OnEnable()
    {
        StartNamedCoroutine("mouse", mouse());
    }

    IEnumerator mouse()
    {
        Player player = enemyTarget.GetComponent<Player>();

        while (true)
        {
            targetDis = Vector2.Distance(transform.position, enemyTarget.position);
            if (isChange == false)
            {
                if(targetDis < 2f)
                {
                    // 물기
                    // 물기 전 대기 시간
                    yield return new WaitForSeconds(0.5f);

                    biteArea.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(enemyTarget.transform.position.y - transform.position.y, enemyTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
                    biteArea.SetActive(true);
                    
                    // 물기 판정 유지 시간
                    yield return new WaitForSeconds(0.5f);

                    biteArea.SetActive(false);

                    // 플레이어 공격 성공 시
                    if(biteArea.GetComponent<HitDetection>().hitSuccess)
                    {
                        Change();
                        yield return new WaitForSeconds(1f);
                    }
                }
                else 
                {
                    Chase();
                }
            }
            else
            {
                //Attack
                if(skill != 0 && skillList[skill].skillCoolTime <= 0)
                {
                    //mimic player skill
                    print("mimic player skill");

                    yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].preDelay : 0);

                    skillList[skill].Enter(gameObject);

                    yield return new WaitForSeconds(skillList[skill].skillType == 0 ? skillList[skill].postDelay : 0);

                    yield return new WaitForSeconds(skillList[skill].skillType != 0 ? skillList[skill].maxHoldTime / 2 : 0);

                    yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].preDelay : 0);

                    skillList[skill].Exit();

                    yield return new WaitForSeconds(skillList[skill].skillType == 2 ? skillList[skill].postDelay : 0);

                }
                else if (targetDis < 6f)
                {
                    targetDirVec = enemyTarget.position - transform.position;
                    transform.Translate(-targetDirVec.normalized * stats.defaultMoveSpeed * Time.deltaTime * 2f);
                }
                else if (targetDis >= 7f)
                {
                    Chase();
                }
                

            }

            yield return null;
        }
    }

    void Change()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = enemyTarget.GetComponentInChildren<SpriteRenderer>().sprite;
        GetComponentInChildren<SpriteRenderer>().transform.localScale = enemyTarget.GetComponentInChildren<SpriteRenderer>().transform.localScale;
        isChange = true;

        skill = enemyTarget.GetComponent<Player>().stats.skill[enemyTarget.GetComponent<Player>().status.skillIndex];
        if (skill != 0) skillList[skill].gameObject.SetActive(true);

        //Run away
        targetDirVec = enemyTarget.position - transform.position;
        rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 10);
    }

}
