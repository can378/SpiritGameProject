using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BossDarkGhost : EnemyBasic
{
    [Header("waveAttack")]
    public GameObject AttackRange;
    public GameObject NoAttackRange;


    void Start()
    {
        StartCoroutine(darkGhost());
    }

    private void OnEnable() 
    {
        StartCoroutine(darkGhost());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator darkGhost()
    {
        //Wave Attack

        NoAttackRange.transform.localScale = new Vector3(0f, 0f, 1f);
        AttackRange.transform.localScale = new Vector3(0f, 0f, 1f);

        float newScale = 1f;
        while (newScale <= 9f)
        {
            newScale += Time.deltaTime;
            AttackRange.transform.localScale = new Vector3(newScale, newScale, 1f);
            if (newScale >= 3f) { NoAttackRange.transform.localScale = new Vector3(newScale - 3f, newScale - 3f, 1f); }
            
            yield return new WaitForSeconds(0.001f);
        }
        while (newScale-3f <= 9f)
        {
            newScale += Time.deltaTime*1.5f;
            NoAttackRange.transform.localScale = new Vector3(newScale - 3f, newScale - 3f, 1f);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(1f);
        
        
        //공격 저하 디버프
        //????

        StartCoroutine(darkGhost());
    }





}
