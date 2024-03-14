using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : EnemyBasic
{
    void Start()
    {
        StartCoroutine(frog());
    }

    private void OnEnable()
    {
        StartCoroutine(frog());
    }

    private void OnDisable()
    {
        StopCoroutine(frog());
    }


    IEnumerator frog()
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        targetDirVec = enemyTarget.position - transform.position;

        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            //get closer to player
            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultSpeed);
            yield return new WaitForSeconds(0.1f);
        }
        else
        {

            //rotate
            /*
            float rotZ = Mathf.Atan2(targetDirVec.y, targetDirVec.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.LookRotation(new Vector3(0,0,rotZ));
            transform.rotation = rotation;
            */


            //run away
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultSpeed * 3);
            yield return new WaitForSeconds(0.1f);


        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(frog());

    }


}