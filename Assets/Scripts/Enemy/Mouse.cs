using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mouse : EnemyBasic
{
    [field : SerializeField] public List<Skill> skillList {get; private set;}
    [field: SerializeField] public int skill {get; private set;}

    private void OnEnable()
    {
        StartCoroutine(mouse());
    }

    private void OnDisable()
    {
        StopCoroutine(mouse());
    }

    private bool isChange=false;

    IEnumerator mouse()
    {
        Player player = enemyTarget.GetComponent<Player>();

        while (true)
        {
            targetDis = Vector2.Distance(transform.position, enemyTarget.position);
            if (isChange == false)
            {
                Chase();
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
                    Chase(); }
                

            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isChange == false)
        {

            //Bite
            //print("Bite");

            //Transform
            GetComponentInChildren<SpriteRenderer>().sprite = enemyTarget.GetComponentInChildren<SpriteRenderer>().sprite;
            GetComponentInChildren<SpriteRenderer>().transform.localScale = enemyTarget.GetComponentInChildren<SpriteRenderer>().transform.localScale;
            isChange = true;

            skill = enemyTarget.GetComponent<Player>().stats.skill[enemyTarget.GetComponent<Player>().status.skillIndex];
            if(skill != 0) skillList[skill].gameObject.SetActive(true);

            //Run away
            targetDirVec = enemyTarget.position - transform.position;
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 100);


        }
        if (collision.tag == "PlayerAttack")
        {
            PlayerAttack(collision.gameObject);
        }

    }

}
