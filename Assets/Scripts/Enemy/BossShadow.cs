using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShadow : EnemyBasic
{
    private int patternNum;

    private void OnEnable()
    {
        StartNamedCoroutine("bossShadow",bossShadow());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator bossShadow() 
    {
        switch (patternNum)
        {
            case 0:break;
            case 1:break;
            case 2:break;
            case 3:break;
            case 4:break;
            case 5:break;
            case 6:break;
            default:break;
        
        }
        yield return null;
    
    }

    IEnumerator spawnShadow() { yield return null; }
}
