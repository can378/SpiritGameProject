using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleMonster : EnemyBasic
{
    public bool isEye;
    public bool isMouth;
    public bool isDetection;

    public GameObject[] mouths;

    private bool isDetected;
    private bool isHit=false;
    void Start()
    {
        if(isEye) { StartCoroutine(eye(new Vector2(1,0))); }
        if (isMouth) { StartCoroutine(mouth()); }
    }
    private void OnEnable()
    {

        if (isEye) { StartCoroutine(eye(new Vector2(1, 0))); }
        if (isMouth) { StartCoroutine(mouth()); }

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator mouth() 
    {
        if (isDetected) 
        {
            Chase();
            print("im mouth");
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(mouth());
        }
        yield return null; 
    }

    IEnumerator eye(Vector2 dir) 
    { 
        if(isDetected) 
        {
            

            //??
        }
        else 
        {
            //wander
            if (isHit == true)
            {
                rigid.velocity = new Vector2(0, 0);
                yield return new WaitForSeconds(0.1f);
                //float randomAngle = Random.Range(0, 360);
                dir *= -1;
                //dir = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;//random direction
                dir += new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                isHit = false;
            }
            rigid.AddForce(dir * GetComponent<EnemyStats>().defaultMoveSpeed * 20);
            yield return new WaitForSeconds(0.1f);
        }

        yield return null; 
        StartCoroutine(eye(dir));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"&&isDetection==true)
        {
            //Eye Scream animation


            isDetected = true;
            for(int i=0;i<mouths.Length;i++) 
            {
                mouths[i].GetComponent<LittleMonster>().isDetected = true;
                StartCoroutine(mouths[i].GetComponent<LittleMonster>().mouth());
            }

            print("Its player!!");
        }
        if (isEye&&collision.tag!="Enemy")
        { isHit = true; }
    }
}
