using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigKnife : MonoBehaviour
{
    private GameObject player;
    private Vector3 moveDir;
    private float targetDis;
    private bool isCoRun;
    private bool isTouchWall;
    private float originalTrailTime;
    void Start()
    {
        isCoRun = false;
        isTouchWall = false;
        player = GameObject.FindWithTag("Player");
        originalTrailTime = GetComponent<TrailRenderer>().time;
        GetComponent<TrailRenderer>().time = 0;
    }


    void Update()
    {
        targetDis = Vector2.Distance(player.transform.position, transform.position);
        moveDir = (player.transform.position - transform.position).normalized;

        if (isCoRun == false)
        {
            if (targetDis > 3f)
            { transform.GetComponent<Rigidbody2D>().AddForce(moveDir * 10); }
            else
            { StartCoroutine(Attack()); }

        }
        

    }

    IEnumerator Attack()
    {
        
        isCoRun = true;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        //Set Vector
        Vector3 vec1 = player.transform.position;
        yield return new WaitForSeconds(1f);
        Vector3 vec2 = player.transform.position;
        Vector3 cutDir = (vec2 - vec1).normalized;

        //�ڱ� ����
        GetComponent<TrailRenderer>().time = originalTrailTime;
        transform.GetComponent<TrailRenderer>().enabled = true;
        
        while (isTouchWall == false)
        {
            transform.GetComponent<Rigidbody2D>().AddForce(cutDir * 30);
            yield return new WaitForSeconds(0.001f);
        }

        //�ڱ� ��
        GetComponent<TrailRenderer>().time = 0;
        transform.GetComponent<TrailRenderer>().enabled = false;
        
        //reset
        isTouchWall = false;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);


        isCoRun = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            isTouchWall = true;
        }
    }
}