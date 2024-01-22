using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPop : EnemyBasic
{

    private SpriteRenderer sprite;
    public float attackDuration = 3f;       // 공격 지속 시간
    private bool isAttacking = false;



    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }


    
    void Update()
    {
        if (isAttacking == false)
        {

            float targetDistance = Vector2.Distance(new Vector2(0,0), enemyTarget.position);
            float targetEnemyDistance = Vector2.Distance(transform.position, enemyTarget.position);

            if (targetDistance <= status.detectionDis)
            {

                if (targetEnemyDistance <= 0.7f)
                {
                    StartCoroutine("attack");
                }
                else 
                {
                    Chase();
                    sprite.color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
            else
            { 
                sprite.color = new Color(1f, 1f, 1f, 0.2f);
            }
        }
    }

    IEnumerator attack() 
    {

        isAttacking = true;
        sprite.color = new Color(1f, 0f, 0f, 0.5f);

        yield return new WaitForSecondsRealtime(attackDuration);

        isAttacking = false;

    }
    


}
