using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShadowDummy : EnemyBasic
{
    private void OnEnable()
    {
        StartNamedCoroutine("shadowDummy", shadowDummy());
        StartNamedCoroutine("shadowShot", shadowShot());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator shadowDummy() 
    {
        Chase();
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(shadowDummy());
    }
    IEnumerator shadowShot() 
    {
        shot();
        yield return new WaitForSeconds(3f);
        StartCoroutine (shadowShot());  
    }
}
