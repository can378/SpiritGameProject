using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{
    /*
    public Transform enemyTarget;


    GameObject enemy;

    private void Start()
    {
        enemy = GameObject.Find("Enemy");

    }

    private void Update()
    {

        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        //target�� ��������� �̵�
        if (targetDistance <= 5 && targetDistance >= 1.2f)
        {
            Chase();
        }
        else //�� �ܿ��� ȥ�� ��ȸ
        {
            Wander();
        }

    }



    //target���� �̵�
    void Chase() 
    {
        Vector2 direction = enemyTarget.transform.position - transform.position;
        transform.Translate(direction *1* Time.deltaTime);

    }
    void Wander()
    {
        Vector2 direction = transform.position;
        direction.x += Random.Range(-2f, 2f);
        direction.y += Random.Range(-2f, 2f);

        transform.Translate(direction * 0.1f*Time.deltaTime);
    
    }
   */

    //��ֹ��� ���ؼ� �̵��ϴ� ���

    //�ִϸ����� ������

    //<Ư��>
    //����, �̸�, ���� ��ġ
    //��, ����(���ݷ�, ���ݼӵ�,���� ����, ���� ��Ÿ�), �̵��ӵ�, Ư�� Ư��

    //<����>
    //������� = ������ / �Ѿư��� / Ž��(����������) / �����ϱ� / �������� / Ư���ൿ
    //���� = ������� ��� / ������ ������ / Ư�� �ൿ
}
