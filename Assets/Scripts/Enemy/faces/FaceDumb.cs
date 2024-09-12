using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDumb : BossFace
{

    //무지=막 돌아다님
    private const float randomMoveCount =5f;
    private float nowCount = 0;
    

    public GameObject floor;
    private Bounds floorBound;
    private bool isHitWall = false;
    private float randomX, randomY;


    public GameObject attackArea;

    
    //start, update/////////////////////////////////////////
    protected override void Start()
    {
        base.Start();
        floorBound = floor.GetComponent<Collider2D>().bounds;
    }
    protected override void Update()
    {
        base.Update();
        attackArea.transform.localPosition = new Vector3(0, 0, 0);
    }

    //Attack////////////////////////////////////////////////
    protected override void faceAttack()
    {
        base.faceAttack();
        attackArea.SetActive(true);
    }


    //Move//////////////////////////////////////////////////
    protected override void MovePattern()
    {
        MadRush();
    }

    void MadRush()
    {
        if(nowCount >= 0)
        {
            if (isHitWall)
            {
                isHitWall = false;

                //벽에 부딫힘
                randomX = Random.Range(floorBound.min.x, floorBound.max.x);
                randomY = Random.Range(floorBound.min.y, floorBound.max.y);
                enemyStatus.moveVec = (new Vector3(randomX, randomY, 0) - transform.position).normalized;
                rigid.velocity = Vector2.zero;
            }
            nowCount -= Time.deltaTime;
        }
        else
        {
            //set new vector
            randomX = Random.Range(floorBound.min.x, floorBound.max.x);
            randomY = Random.Range(floorBound.min.y, floorBound.max.y);
            enemyStatus.moveVec = (new Vector3(randomX, randomY, 0) - transform.position).normalized;
            rigid.velocity = Vector2.zero;
            nowCount = randomMoveCount;
        }

    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);


        if (collision.CompareTag("Wall")) 
        {
            isHitWall = true;
        }
    }
}
