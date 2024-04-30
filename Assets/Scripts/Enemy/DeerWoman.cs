using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerWoman : EnemyBasic
{


    private void OnEnable()
    {
        StartNamedCoroutine("deerWoman", deerWoman());
    }



    IEnumerator deerWoman()
    {
        //shot arrow
        GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
        bullet.transform.position = transform.position;


        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

        targetDirVec = (enemyTarget.position - transform.position).normalized;
        rigidBullet.AddForce(targetDirVec.normalized * 3, ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);

        StartCoroutine(deerWoman());

    }
}
