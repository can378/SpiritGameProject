using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : EnemyBasic
{
    public LineCreator lineCreator;

    void Start()
    {    StartCoroutine(frog());   }

    private void OnEnable()
    {    StartCoroutine(frog());    }

    private void OnDisable()
    {    StopCoroutine(frog());    }


    IEnumerator frog()
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        targetDirVec = enemyTarget.position - transform.position;


        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            lineCreator.currentLineLength = 0f;
            StopCoroutine(lineCreator.laserBeam());
            lineCreator.isLaserBeamRun = false;
            lineCreator.transform.gameObject.SetActive(false);

            //get closer to player
            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultSpeed);
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            if (lineCreator.isLaserBeamRun==false)
            {
                //attack
                StartCoroutine(lineCreator.laserBeam());
                //run away
                /*
                rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultSpeed * 10);
                yield return new WaitForSeconds(0.1f);
                */
            }
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(frog());


    }


}