using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : EnemyBasic
{
    private void OnEnable()
    {
        StartNamedCoroutine("worm", worm());

    }
    

    IEnumerator worm()
    {
        if (Vector2.Distance(enemyTarget.transform.position, transform.position) < 1f)
        {
            //stick to player
            transform.position = enemyTarget.transform.position;
            sprite.sortingOrder = enemyTarget.GetComponent<SpriteRenderer>().sortingOrder + 1;

            //player move slowly
            enemyTarget.GetComponent<PlayerStats>().defaultMoveSpeed -= 3;
            yield return new WaitForSeconds(3f);
            enemyTarget.GetComponent<PlayerStats>().defaultMoveSpeed += 3;
            Destroy(this.gameObject);
        }
        else
        {
            //chase
            Chase();
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(worm());
        
    }

   


}
