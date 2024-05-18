using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossShaman : EnemyBasic
{
    private int attackNum=0;
    public GameObject dollPrefab;

    protected override void AttackPattern()
    {
        StartCoroutine(bossShaman());
    }



    IEnumerator bossShaman()
    {
        isAttack = true;
        isAttackReady = false;

        switch (attackNum)
        {
            case 0:
                StartCoroutine(SummonGhost());
                break;
            case 1:
                StartCoroutine(SummonDoll());
                break;
            case 2:
                StartCoroutine(ShotKnife());
                break;
            default:
                break;
        }
        yield return null;
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

        attackNum = 1;
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator SummonDoll()
    {
        GameObject doll=Instantiate(dollPrefab);
        doll.transform.position=transform.position;
        yield return new WaitForSeconds(3f);

        attackNum = 2;
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
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

        attackNum = 0;
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

}
