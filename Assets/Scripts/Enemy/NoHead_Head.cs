using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHead_Head : EnemyBasic
{
    public LineCreator lineCreator;
    public GameObject vomit;
    void Start()
    { StartCoroutine(head()); }

    private void OnEnable()
    { StartCoroutine(head()); }

    private void OnDisable()
    { StopCoroutine(head()); }



    IEnumerator head() 
    {
        print("head vomit");
        //targetDirVec = (enemyTarget.position - transform.position).normalized;

        float angle = Mathf.Atan2
            (enemyTarget.transform.position.y - transform.position.y,
             enemyTarget.transform.position.x - transform.position.x)
           * Mathf.Rad2Deg;
        
        vomit.transform.rotation = Quaternion.Euler(0, 0, angle-180);


       // Quaternion rotation = Quaternion.LookRotation(0,0,targetDirVec.z);
        //vomit.transform.rotation = rotation;


        vomit.SetActive(true);
        yield return new WaitForSeconds(3f);
        vomit.SetActive(false);
        yield return new WaitForSeconds(3f);

        StartCoroutine(head());
    }
    
}
