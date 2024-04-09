using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossShaman : EnemyBasic
{
    public GameObject dollPrefab;
    void Start()
    {

        StartCoroutine(SummonGhost());
    }

    private void OnEnable()
    {
        //StartCoroutine(SummonGhost());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }



    IEnumerator SummonGhost()
    {
        for(int i=0;i < 3; i++) 
        {
            GameObject ghost = ObjectPoolManager.instance.Get2("ghost");
            ghost.transform.position = transform.position;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(2f);

        StartCoroutine(SummonDoll());
    }

    IEnumerator SummonDoll()
    {
        GameObject doll=Instantiate(dollPrefab);
        doll.transform.position=transform.position;
        yield return new WaitForSeconds(3f);
        StartCoroutine(ShotKnife());
    }

    IEnumerator ShotKnife()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject knife = ObjectPoolManager.instance.Get2("knife");
            knife.transform.position = transform.position;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);

        StartCoroutine(SummonGhost());
    }

}
