using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAngry : BossFace
{
    //�ڵ�� �۵��� ������ ���� ������



    //�г�=�Ӹ��� ��, �� ���� �� ������ �������� ���ƴٴ�

    public List<GameObject> fires;//�ƴ� �̰� �θ� �ڽ� �����ε� ������ ������ϴ°��� ���ذ� �Ȱ��µ� �ϴ� �ȵǼ� �̷��� ��;;;
    public GameObject fire;

    private const float randomMoveCount = 3f;
    private float nowCount = 0;

    public GameObject floor;
    private Bounds floorBound;
    private bool isHitWall = false;
    private float randomX, randomY;

    protected override void Start()
    {
        base.Start();
        floorBound = floor.GetComponent<Collider2D>().bounds;
    }

    protected override void Update()
    {
        base.Update();
        fires[0].transform.localPosition = new Vector3(-1.85f, -0.03f, 0);
        fires[1].transform.localPosition = new Vector3(-0.007f, 1.9f, 0);
        fires[2].transform.localPosition = new Vector3(1.72f, -0.001f, 0);

    }

    protected override void Move()
    {
        if(isFaceApproach)
        {
            //Approaching
            if (countTime > 0)
            {
                MoveTo(gameObject, 2.5f, transform.position, FindObj.instance.Player.transform.position);
                countTime--;

            }
            else { print("angry set attack"); setAttack(); }

        }
        else if (isFaceBack)
        {
            //Back
            if (Vector2.Distance(transform.position, originalPose.transform.position) >= 1f)
            {
                Vector3 vec = (originalPose.transform.position - transform.position).normalized;
                GetComponent<Rigidbody2D>().AddForce(vec * 2.5f);
            }
            else { print("angry set finish"); Finish(); }
        }
        else if (isFaceAttack)
        {
            
            // ���� �߿��� ���� �̵� �Ұ�
            if (0 < enemyStatus.isFlinch)
            {
                return;
            }

            MovePattern();

            rigid.velocity = enemyStatus.moveVec * stats.MoveSpeed.Value;
        }


    }

    protected override void MovePattern() { }


    private void madRush() {

        print("angry move");
        if (nowCount >= 0)
        {
            if (isHitWall)
            {
                isHitWall = false;

                //���� �΋H��
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



    protected override void Detect()
    {
        enemyStatus.EnemyTarget = FindObj.instance.Player.GetComponent<ObjectBasic>();
    }

    protected override void faceAttack()
    {
        //print("angry attack");
        base.faceAttack();
        madRush();
        fire.SetActive(true);
       
    }
    protected override void init()
    {
        base.init();
        nowCount = randomMoveCount;
        enemyStatus.moveVec = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;
    }
    protected override void Finish()
    {
        base .Finish();
        rigid.velocity = Vector2.zero;
        fire.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);


        if (collision.CompareTag("Wall"))
        {
            isHitWall = true;
        }
    }
















    // protected override void Move()
    // {
    //     // ���� �߿��� ���� �̵� �Ұ�
    //     if (enemyStatus.isFlinch)
    //     {
    //         return;
    //     }
    //     else if (enemyStatus.isRun)
    //     {
    //         if (enemyStatus.enemyTarget)
    //         {
    //             rigid.velocity = -(enemyStatus.enemyTarget.position - transform.position).normalized * stats.moveSpeed;
    //         }
    //         return;
    //     }

    //     MovePattern();

    //     rigid.velocity = enemyStatus.moveVec * stats.moveSpeed;

    // }





    //�̰Ŵ� �� ������..?
    /*
    IEnumerator angry()
    {
        //enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        foreach(GameObject f in faces) 
        {
            f.SetActive(true);
        }

        yield return new WaitForSeconds(3f);
        foreach (GameObject f in faces)
        {
            f.SetActive(false);
        }



        //enemyStatus.isAttack = false;
        yield return new WaitForSeconds(1f);
        enemyStatus.isAttackReady = true;

    }
    */
}
