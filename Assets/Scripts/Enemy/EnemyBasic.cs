using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{
    //<����>
    //����, �̸�, ���� ��ġ
    //��, ����(���ݷ�, ���ݼӵ�,���� ����, ���� ��Ÿ�), �̵��ӵ�, Ư�� Ư��

    public Transform enemyTarget;
    public string name;
    public Sprite sprite;
    public Vector2 spawnPosition;
    public int health;
    public float speed;
    public int attack;
    

    GameObject enemy;

    private void Start()
    {
        enemy = GameObject.Find("Enemy");

    }

    private void Update()
    {

        //�̵� ����? ��ֹ� ���ϰų� ������ �ʰ� �̵�

        //�ִϸ�����
        if (health < 1f) { EnemyDead(); }
        else 
        {
            //������� = ������ / �Ѿư��� / Ž��(����������)
            //�����ϱ� / �������� / Ư���ൿ

        }


    }

    public void EnemyDead() 
    {
        Debug.Log(this.gameObject.name + "�� �׾���.");
        //�״� ���
        //������ ������
        //���Ϳ� ���� Ư�� �ൿ?
        //�� ���� ������Ʈ �ı�?
    
    }

    //�÷��̾�� ������ ������ �ִ´�
    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            print("�÷��̾�� ����. �䱫�� �������� �ִ´�.");
        }
    }*/

    void Chase()
    {
        Vector2 direction = enemyTarget.transform.position - transform.position;
        transform.Translate(direction * speed * Time.deltaTime);

    }
}
