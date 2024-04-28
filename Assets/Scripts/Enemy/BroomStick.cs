using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomStick : EnemyBasic
{

    public GameObject colObj;
    public GameObject buff;

    void Start()
    {
        StartCoroutine(peripheralAttack(colObj, 50, 2));
    }
    private void OnEnable()
    {
        StartCoroutine(peripheralAttack(colObj, 50, 2));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    //본인의 주변을 공격
    public IEnumerator peripheralAttack(GameObject colObj, float radius, float attackTime)
    {
        //attack
        colObj.transform.position = transform.position;
        colObj.SetActive(true);
        
        float r = 1;
        while (r < radius)
        {
            colObj.transform.localScale = new Vector3(r, r, 1);
            r += 1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(attackTime);

        //no attack
        colObj.transform.localScale = new Vector3(1,1, 1);
        colObj.SetActive(false);

        //chase
        for (int i = 0; i < 10; i++) 
        { 
            Chase();
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2f);


        StartCoroutine(peripheralAttack(colObj, radius, attackTime));

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //두통 디버프 5초
            enemyTarget.GetComponent<Player>().ApplyBuff(buff);

        }
    }
}
