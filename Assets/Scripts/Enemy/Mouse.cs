using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mouse : EnemyBasic
{
    private List<GameObject> skillList;

    void Start()
    {
        skillList = DataManager.instance.gameData.skillList;
        StartCoroutine(mouse());
    }

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
        targetDis=Vector2.Distance(transform.position, enemyTarget.position);
        if (isChange == false)
        {
            Chase();
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            //Attack
            if (targetDis > 3f) 
            { Chase(); yield return new WaitForSeconds(0.1f); }
            else
            {
                
                
                if (player.stats.skill[player.status.skillIndex] != 0)
                {
                    //mimic player skill
                    print("mimic player skill");
                    
                    for (int i = 0; i < skillList.Count; i++)
                    {
                        if (skillList[i].GetComponent<Skill>().skillID == player.stats.skill[player.status.skillIndex])
                        {
                            enemyTarget.GetComponent<Player>().skillController.skillList[player.stats.skill[player.status.skillIndex]].GetComponent<Skill>().Use(gameObject);
                            break;
                            //print("skill=" + skillList[i].GetComponent<Skill>().skillName); 
                        
                        }

                    }

                    yield return new WaitForSeconds(3f);
                   
                }
                else 
                {
                    //chaos
                    print("chaos");
                }

                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                yield return new WaitForSeconds(0.1f);

                //run away
                while (targetDis<3f)
                {
                    targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                    targetDirVec = enemyTarget.position - transform.position;
                    rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 10);
                    yield return new WaitForSeconds(0.1f);
                }
            }

        }
        
        yield return null;
        StartCoroutine(mouse());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isChange == false)
        {

            //Bite
            //print("Bite");

            //Transform
            GetComponent<SpriteRenderer>().sprite = enemyTarget.GetComponent<SpriteRenderer>().sprite;
            //transform.localScale = enemyTarget.transform.localScale;
            isChange = true;

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
