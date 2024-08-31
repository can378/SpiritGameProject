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
    public TrailRenderer trailRender;
    public EdgeCollider2D edgeCollider;

    void Start()
    {
        isCoRun = false;
        isTouchWall = false;

        player = FindObj.instance.Player;

        originalTrailTime = trailRender.time;
        trailRender.time = 0;
    }


    void Update()
    {
        targetDis = Vector2.Distance(player.transform.position, transform.position);
        moveDir = (player.transform.position - transform.position).normalized;

        if (isCoRun == false)
        {
            if (targetDis > 6f)
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

        if (vec2 == vec1) { vec1 = transform.position; }
        Vector3 cutDir = (vec2 - vec1).normalized;

        //자국 시작
        trailRender.time = originalTrailTime;
        //trailRender.enabled = true;
        //edgeCollider.enabled = true;


        while (isTouchWall == false)
        {
            transform.GetComponent<Rigidbody2D>().AddForce(cutDir * 30);
            yield return new WaitForSeconds(0.001f);
        }

        //자국 끝
        trailRender.time = 0;
        //trailRender.enabled = false;
        //edgeCollider.enabled = false;

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
