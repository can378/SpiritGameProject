using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : EnemyBasic
{
    public GameObject laser;
    public LineCreator lineCreator;



    private void OnEnable()
    { StartNamedCoroutine("frog", frog()); }



    IEnumerator frog()
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        targetDirVec = enemyTarget.position - transform.position;


        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            //no laser shooting
            lineCreator.currentLineLength = 0f;
            StopCoroutine(lineCreator.laserBeam());
            lineCreator.isLaserBeamRun = false;
            laser.SetActive(false);

            //get closer to player
            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed);
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            if (lineCreator.isLaserBeamRun==false)
            {
                //shooting laser
                laser.SetActive(true);
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