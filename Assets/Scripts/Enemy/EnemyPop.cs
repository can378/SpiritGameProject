using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPop : EnemyBasic
{

    private GameObject Target;
    private SpriteRenderer sprite;


    public float attackRange = 5f;        // ���� ����
    public float waitTimeBeforeAttack = 3f; // Ƣ����� �� ��� �ð�
    public float attackDuration = 3f;       // ���� ���� �ð�

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
                    // �÷��̾ ���� ���� �̳��� �ִ� --> ��� ����
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

        

        // ���� �ð� �ڿ� ���� ����
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
        // obj1�� obj2�� Collider2D�� ��ġ���� ���θ� ��ȯ
        return obj1.GetComponent<Collider2D>().IsTouching(obj2.GetComponent<Collider2D>());
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, Target.transform.position, speed * Time.deltaTime);

    }


}
