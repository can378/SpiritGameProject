using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWhiteTiger : EnemyBasic
{
    public GameObject splitSnowArea;
    public GameObject attackArea;
    public GameObject screamArea;
    public List<GameObject> dummies = new List<GameObject>();

    private int patternNum;
    private int time = 0;

    private void OnEnable()
    {
        patternNum = 0;
        time = 0;
        StartNamedCoroutine("whiteTiger", whiteTiger());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator whiteTiger()
    {
        patternNum++;

        switch (patternNum)
        {
            case 1:
                StartCoroutine(contiAttack());
                break;
            case 2:
                StartCoroutine(eating());
                break;
            case 3:
                StartCoroutine(splitSnow());
                break;
            case 4:
                StartCoroutine(scream());
                break;
            default:
                patternNum = 0;
                StartCoroutine(spawnDummy());
                break;

        }
        yield return null;

    }



    IEnumerator contiAttack()
    {
        while (Vector2.Distance(enemyTarget.transform.position, transform.position) > 5f)
        { 
            Chase();
            yield return new WaitForSeconds(0.1f);
        }
        

        for (int i = 0; i < 4; i++)
        {
            //할퀴거나 내리친다 --> 애니메이션필요
            attackArea.SetActive(true);
            yield return new WaitForSeconds(1f);
            attackArea.SetActive(false);
        }

        
        StartCoroutine(whiteTiger());
    }



    IEnumerator eating() 
    {

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(whiteTiger());
    }

    IEnumerator scream()
    {
        while (time < 100)
        {
            screamArea.SetActive(true);
            //점점커지게 해야하나
            time++;
        
        }

        screamArea.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(whiteTiger());

    }
    IEnumerator spawnDummy()
    {
        //Boss shadow invincible
        //?????



        //spawn dummy
        for (int i = 0; i < dummies.Count; i++)
        {
            dummies[i].SetActive(true);
        }

        bool isDummiesAllDie = false;
        while (isDummiesAllDie == false)
        {
            //attack
            Chase();
            shot();

            //check dummy
            for (int i = 0; i < dummies.Count; i++)
            {
                if (dummies[i].GetComponent<EnemyStats>().HP > 0)
                {
                    break;
                }
                if (i == dummies.Count - 1)
                { isDummiesAllDie = true; }
            }
            yield return new WaitForSeconds(0.01f);
        }


        //Boss shadow vincible
        //????????
        StartCoroutine(whiteTiger());
    }



    IEnumerator splitSnow()
    {
        splitSnowArea.SetActive(true);
        yield return new WaitForSeconds(5f);
        splitSnowArea.SetActive(false);
        
        StartCoroutine(whiteTiger());

    }
}
