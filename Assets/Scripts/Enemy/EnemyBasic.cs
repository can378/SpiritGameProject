using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{

    public Transform enemyTarget;
    public Vector2 spawnPosition;
    public int health;
    public float walkSpeed;
    //public float runSpeed;
    //public int attackSpeed;
    //public int attackPower;
    //public int attackRange;
    public int detectionDistance;

    



    private void Start()
    {
        enemyTarget=GameObject.Find("Player").transform;
        this.transform.position = spawnPosition;
    }

    private void Update()
    {

        //�̵� ����? ��ֹ� ���ϰų� ������ �ʰ� �̵�

        //�ִϸ�����

        //Death
        if (health < 0.1f) { EnemyDead(); }
        else 
        {
            //������� = ������ / �Ѿư��� / Ž��(����������)
            //�����ϱ� / �������� / Ư���ൿ

        }


    }

    public void EnemyDead() 
    {
        Debug.Log(this.gameObject.name + "�� �׾���.");
        //animation

        //drop item
        //���Ϳ� ���� Ư�� �ൿ?
        //�� ���� ������Ʈ �ı�?
    
    }



    public void Chase()
    {
        Vector2 direction = enemyTarget.transform.position - transform.position;
        transform.Translate(direction * walkSpeed * Time.deltaTime);

    }


    public void Wander()
    {
        /*
        Vector2 direction = transform.position;
        direction.x += Random.Range(-2f, 2f);
        transform.Translate(direction * 0.1f * Time.deltaTime);
        */
    }
}
