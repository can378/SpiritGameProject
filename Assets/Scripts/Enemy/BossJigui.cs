using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossJigui : EnemyBasic
{
    public GameObject fire;
    public GameObject eyeSight;

    void Start()
    {
        StartCoroutine(jigui());
        StartCoroutine(jiguiRaid());
        StartCoroutine(playerEyeSight());
    }

    private void OnEnable()
    {
        //StartCoroutine(jigui());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator jigui()
    {
        //Throw fire balls
        for (int i = 0; i < 20; i++)
        {
            GameObject fireBall = ObjectPoolManager.instance.Get2("fireBall");
            fireBall.transform.position = transform.position;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(3f);

        //Fire Area attack
        fire.SetActive(true);
        fire.transform.position = new Vector3(0, 0, 0);
        fire.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        yield return new WaitForSeconds(2f);
        fire.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(2f);
        fire.SetActive(false);
        //fire에 플레이어가 닿으면 불붙게하는 너프 적용!!!!!!!!!!!!!

        StartCoroutine(jigui());
    }

    IEnumerator jiguiRaid()
    {
        if (!eyeSight.GetComponent<EyeSight>().isPlayerSeeEnemy)
        {
            Vector2 originPos = transform.position;

            
            targetDis = Vector2.Distance(enemyTarget.transform.position, transform.position);

            while (targetDis > 0.1f)
            {
                targetDirVec = enemyTarget.transform.position - transform.position;
                targetDis = Vector2.Distance(enemyTarget.transform.position, transform.position);
                rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed*10);
                yield return new WaitForSeconds(0.1f);
            }

            Vector2 currentPos = transform.position;
            Vector2 dirVec = (originPos - currentPos).normalized;

            while (Vector2.Distance(transform.position, originPos)>0.1f)
            {
                rigid.AddForce(dirVec * GetComponent<EnemyStats>().defaultMoveSpeed*10);
                currentPos = transform.position;
                dirVec = (originPos - currentPos).normalized;
                yield return new WaitForSeconds(0.1f);
            }

        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(jiguiRaid());
    }

    IEnumerator playerEyeSight() 
    {
        eyeSight.transform.position = enemyTarget.transform.position;

        if (Player.instance.hAxis == -1) 
        { 
            eyeSight.GetComponent<SpriteRenderer>().flipX = true; 
            eyeSight.transform.rotation = Quaternion.Euler(0, 0, 0); 
        }
        else if (Player.instance.hAxis == 1) 
        { 
            eyeSight.GetComponent<SpriteRenderer>().flipX = false;
            eyeSight.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Player.instance.vAxis == -1) 
        {
            eyeSight.GetComponent<SpriteRenderer>().flipX = true;
            eyeSight.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Player.instance.vAxis == 1) 
        {
            eyeSight.GetComponent<SpriteRenderer>().flipX = true;
            eyeSight.transform.rotation = Quaternion.Euler(0, 0, -90);
        }


        yield return new WaitForSeconds(0.1f);
        StartCoroutine(playerEyeSight());
    }


}
