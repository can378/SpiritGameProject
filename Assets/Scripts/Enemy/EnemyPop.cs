using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPop : EnemyBasic
{

    private GameObject Target;
    private SpriteRenderer sprite;


    public float attackRange = 5f;        // 공격 범위
    public float waitTimeBeforeAttack = 3f; // 튀어나오기 전 대기 시간
    public float attackDuration = 3f;       // 공격 지속 시간

    private bool isWaiting = false;
    private bool isAttacking = false;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Target = GameObject.Find("Player");
       
    }


    
    void Update()
    {

        if (!isAttacking)
        {
            if (!isWaiting)
            {
                
                if (Vector2.Distance(transform.position, Target.transform.position) <= attackRange)
                {
                    // 플레이어가 공격 범위 이내에 있다 --> 대기 시작
                    isWaiting = true;
                    Invoke("StartAttack", waitTimeBeforeAttack);
                }
            }
        }



    }


    void StartAttack()
    {
        isWaiting = false;
        isAttacking = true;

        sprite.color = new Color(1f, 1f, 1f, 1f);

        

        // 일정 시간 뒤에 공격 종료
        Invoke("StopAttack", attackDuration);
    }

    void StopAttack()
    {
        isAttacking = false;
        transform.position = new Vector2(Target.transform.position.x, Target.transform.position.y);
        sprite.color = new Color(1f, 1f, 1f, 0.5f);
    }




















    bool CheckCollision(GameObject obj1, GameObject obj2)
    {
        // obj1과 obj2의 Collider2D가 겹치는지 여부를 반환
        return obj1.GetComponent<Collider2D>().IsTouching(obj2.GetComponent<Collider2D>());
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, Target.transform.position, speed * Time.deltaTime);

    }


}
