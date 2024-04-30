using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTree : EnemyBasic
{

    private void OnEnable()
    {
        StartNamedCoroutine("fakeTree", fakeTree());
    }


    IEnumerator fakeTree()
    {
        targetDirVec = (enemyTarget.position - transform.position).normalized;
        targetDis = Vector2.Distance(enemyTarget.position, transform.position);

        if (targetDis > 5f)
        {
            //normal form
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        
        }
        else 
        {
            //transform
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(fakeTree());

    }



}
