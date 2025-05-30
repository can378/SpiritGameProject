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
    public List<GameObject> zones;


    protected override void AttackPattern()
    {
        StartCoroutine(darkGhost());
    }


    IEnumerator darkGhost()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        //Wave Attack
        NoAttackRange.SetActive(true);
        AttackRange.SetActive(true);

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
        

        NoAttackRange.SetActive(false);
        AttackRange.SetActive(false);

        yield return new WaitForSeconds(3f);

        //Zone Attack
        for (int i = 0; i < zones.Count; i++)
        {
            zones[i].SetActive(true);
            zones[i].transform.position = enemyStatus.EnemyTarget.transform.position;
            yield return new WaitForSeconds(1f);
        
        }

        yield return new WaitForSeconds(2f);
        for (int i = 0; i < zones.Count; i++)
        {
            zones[i].SetActive(false);
        }
        yield return new WaitForSeconds(2f);


        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }





}
