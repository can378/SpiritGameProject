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
        eyeSight.SetActive(true); 
        StartCoroutine(jigui());
        StartCoroutine(jiguiRaidStart());
        StartCoroutine(playerEyeSight());
    }

    private void OnEnable()
    {
        eyeSight.SetActive(true);
        StartCoroutine(jigui());
        StartCoroutine(jiguiRaidStart());
        StartCoroutine(playerEyeSight());
    }
    private void OnDisable()
    {
        eyeSight.SetActive(false);
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
        fire.transform.position = transform.position;
        fire.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        yield return new WaitForSeconds(2f);
        fire.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(2f);
        fire.SetActive(false);
        //fire에 플레이어가 닿으면 불붙게하는 너프 적용!!!!!!!!!!!!!

        StartCoroutine(jigui());
    }

    IEnumerator jiguiRaidStart() 
    {
        yield return new WaitForSeconds(4f);
        StartCoroutine(jiguiRaid());
    }
    IEnumerator jiguiRaid()
    {
        print("jigui raid="+ eyeSight.GetComponent<EyeSight>().isPlayerSeeEnemy);
        if (eyeSight.GetComponent<EyeSight>().isPlayerSeeEnemy==false)
        {
            Vector2 originPos = transform.position;

            
            targetDis = Vector2.Distance(enemyTarget.transform.position, transform.position);

            while (targetDis > 3f)
            {
                print("jigui raid - run to player");
                targetDirVec = enemyTarget.transform.position - transform.position;
                targetDis = Vector2.Distance(enemyTarget.transform.position, transform.position);
                rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed);
                yield return new WaitForSeconds(0.1f);
            }

            Vector2 currentPos = transform.position;
            Vector2 dirVec = (originPos - currentPos).normalized;

            while (Vector2.Distance(transform.position, originPos)>3f)
            {
                print("jigui raid - run away");
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


        if (Player.instance.hAxis == 1) 
        {

            transform.localScale = new Vector3(-1f, 1f, 1f);
            eyeSight.transform.rotation = Quaternion.Euler(0, 0, 180);
            
        }
        else if (Player.instance.hAxis == -1) 
        { 

            transform.localScale = new Vector3(-1f, 1f, 1f);
            eyeSight.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Player.instance.vAxis == 1) 
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            eyeSight.transform.rotation = Quaternion.Euler(0, 0, -90);
            
        }
        else if (Player.instance.vAxis == -1) 
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            eyeSight.transform.rotation = Quaternion.Euler(0, 0, 90);
            
        }


        yield return new WaitForSeconds(0.1f);
        StartCoroutine(playerEyeSight());
    }


}
