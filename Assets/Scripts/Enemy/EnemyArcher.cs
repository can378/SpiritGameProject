using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : EnemyBasic
{
    SpriteRenderer spriteR;

    public GameObject fireBall;
    public GameObject[] fireBalls;

    int objectCount = 10;
    private int pivot = 0;
    public float attackDelay = 2f;

    private float lastAttackTime;





    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
    }
    void Start()
    {


        //fire ball �̸� ����, object pool
        fireBalls = new GameObject[objectCount];
        for (int i = 0; i < objectCount; i++)
        {
            GameObject gameObject = Instantiate(fireBall);
            fireBalls[i] = gameObject;
            gameObject.SetActive(false);
            gameObject.transform.parent = this.gameObject.transform;
            gameObject.transform.position = gameObject.transform.parent.position;
        }
        
    }


    void Update()
    {
        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);


        if (targetDistance <= 7 && targetDistance >= 0f)
        {
            // ���� �������� ���� ����
            /*
            if (Time.time - lastAttackTime > attackDelay)
            {
                Attack();
                lastAttackTime = Time.time;
            }
            */
            spriteR.color = Color.red;
            StartCoroutine("ActiveFire");

        }
        else 
        {
            spriteR.color = Color.white;
            StopCoroutine("ActiveFire");
        }
        


    }


    IEnumerator ActiveFire()
    {
        yield return new WaitForSeconds(2f);
        
        fireBalls[pivot++].SetActive(true);
        if (pivot == objectCount) pivot = 0;

        StartCoroutine("ActiveFire");
    }
    

}
