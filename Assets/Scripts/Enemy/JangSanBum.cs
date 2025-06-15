using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class JangSanBum : EnemyBasic
{
    enum JangSanBumHitEffect { Scratch, Bite, Snow, Crack, None };

    [SerializeField] GameObject[] spawnCandidate;
   
    public GameObject spawnCandidateParent;

    
    private int patternNum;
    private int blindTime = 5;
    //private int spawnEnemyHP = 50;

    int randomNum;

    private GameObject floor;
    float randomX, randomY;
    Bounds bounds;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        floor = GameManager.instance.nowRoom;
        bounds = floor.GetComponent<Collider2D>().bounds;

        patternNum = 0;

    }

    protected override void Update()
    {
        base.Update();
        checkBite();
    }

    protected override void AttackPattern()
    {
        patternNum++;
        switch (patternNum)
        {
            case 1:
                enemyStatus.attackCoroutine = StartCoroutine(FastAttack());
                break;
            case 2:
                enemyStatus.attackCoroutine = StartCoroutine(Eating());
                break;
            case 3:
                enemyStatus.attackCoroutine = StartCoroutine(SnowSplash());
                break;
            case 4:
                enemyStatus.attackCoroutine = StartCoroutine(RandomSpawn());
                break;
            default:
                patternNum = 0;
                break;
        }
    }

    IEnumerator FastAttack()
    {
        //enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        print("faset attack");
        int time = UnityEngine.Random.Range(3,5);
        while(time > 0)
        {
            //멀리 있다면 연속으로 할퀴며 다가간다

            float moveTime = 0.2f;
            //move to player
            while (moveTime > 0)
            {
                rigid.AddForce(enemyStatus.targetDirVec * enemyStats.MoveSpeed.Value * 100);
                moveTime -= Time.deltaTime;
                yield return null;
            }

            //attack
            hitEffects[(int)JangSanBumHitEffect.Scratch].transform.rotation = enemyStatus.targetQuaternion;
            hitEffects[(int)JangSanBumHitEffect.Scratch].SetActive(true);
            yield return new WaitForSeconds(0.6f);
            hitEffects[(int)JangSanBumHitEffect.Scratch].SetActive(false);
            yield return new WaitForSeconds(0.2f);

            time--;
        }

        //근처에 있으면 내리친다
        if (enemyStatus.targetDis < 4f)
        {
            //attack
            hitEffects[(int)JangSanBumHitEffect.Crack].transform.rotation = enemyStatus.targetQuaternion;
            hitEffects[(int)JangSanBumHitEffect.Crack].SetActive(true);
            yield return new WaitForSeconds(3f);
            hitEffects[(int)JangSanBumHitEffect.Crack].SetActive(false);
        }
        else
        {
            for (int j = 0; j < 5; j++)
            {
                rigid.AddForce(enemyStatus.targetDirVec * enemyStats.MoveSpeed.Value * 500);
                yield return new WaitForSeconds(0.01f);
            }
            hitEffects[(int)JangSanBumHitEffect.Scratch].transform.rotation = enemyStatus.targetQuaternion;
            hitEffects[(int)JangSanBumHitEffect.Scratch].SetActive(true);
            yield return new WaitForSeconds(0.6f);
            hitEffects[(int)JangSanBumHitEffect.Scratch].SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }

        //enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }

    IEnumerator Eating() 
    {
        print("eating");
        //쫓아간다. 잡아먹으려한다.
        //잡아먹으면 플레이어 큰 피해입고 장산범은 일부 체력회복
        enemyStatus.isAttackReady = false;

        enemyStatus.moveVec = (enemyStatus.EnemyTarget.transform.position - transform.position).normalized * 3f;
        hitEffects[(int)JangSanBumHitEffect.Bite].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(enemyStatus.targetDirVec.y, enemyStatus.targetDirVec.x) * Mathf.Rad2Deg - 90);

        //입을 크게 벌리는 모션
        yield return new WaitForSeconds(1f);
        hitEffects[(int)JangSanBumHitEffect.Bite].SetActive(true);

        yield return new WaitForSeconds(1f);
        hitEffects[(int)JangSanBumHitEffect.Bite].SetActive(false);
        enemyStatus.moveVec = Vector2.zero;

        yield return new WaitForSeconds(1f);

        enemyStatus.isAttackReady = true;
    }


    IEnumerator SnowSplash() 
    {
        print("snow splash");
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;


        //플레이어 방향으로 넓은 범위에 눈 뿌린다. 눈에 맞으면 잠시 실명
        yield return new WaitForSeconds(0.4f);
        hitEffects[(int)JangSanBumHitEffect.Snow].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(enemyStatus.targetDirVec.y, enemyStatus.targetDirVec.x) * Mathf.Rad2Deg - 90);
        hitEffects[(int)JangSanBumHitEffect.Snow].SetActive(true);

        yield return new WaitForSeconds(2f);
        hitEffects[(int)JangSanBumHitEffect.Snow].SetActive(false);


        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }


    IEnumerator RandomSpawn()
    {
        print("random spawn");



        /*
        for (int i = 0; i < 4; i++)
        {
            //print(spawnCandidate[i].name);
            randomNum = UnityEngine.Random.Range(0, spawnCandidate.Length);
            spawnEnemy[i] = spawnCandidate[randomNum];
            spawnEnemy[i].SetActive(true);
            spawnEnemy[i].transform.position = new Vector2(randomX, randomY);
        }
        */


        //SPAWN ENEMY
        List<GameObject> spawnEnemies = new List<GameObject>();
        gameObject.transform.position = new Vector3(-350, -1.4f, 0f);//장산범 치워두기
        foreach(GameObject obj in spawnCandidate) 
        {
            if (obj != null) 
            {
                GameObject newObj = Instantiate(obj);
                randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
                randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
                newObj.transform.position=new Vector2(randomX, randomY);
                spawnEnemies.Add(newObj);
            }
        }

        //CHECK
        //randumNum 번째 enemy가 변신한 장산범이므로 그 것을 죽여야지 끝난다.
        randomNum = UnityEngine.Random.Range(0, spawnEnemies.Count);
        print("real jangsanbum num=" + spawnEnemies[randomNum].name);

        while (spawnEnemies[randomNum]!=null)
        {
            if (spawnEnemies[randomNum].GetComponent<EnemyStats>().HP <=1) { break; }
            yield return new WaitForSeconds(0.1f);
        }

        foreach (GameObject obj in spawnEnemies)
        {
            if(obj!=null) { Destroy(obj); }
            
        }
        spawnEnemies.Clear();
        /*
        spawnCandidateParent.SetActive(true);
        //랜덤 적 소환
        for (int i = 0; i < spawnCandidate.Length; i++)
        {

            randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
            spawnCandidate[i].transform.position = new Vector2(randomX, randomY);

            spawnCandidate[i].GetComponent<EnemyStats>().HP = spawnCandidate[i].GetComponent<EnemyStats>().HPMax;
        }



        //CHECK
        //randumNum 번째 enemy가 변신한 장산범이므로 그 것을 죽여야지 끝난다.
        randomNum = UnityEngine.Random.Range(0, spawnCandidate.Length);
        print("real jangsanbum num="+ spawnCandidate[randomNum].name);

        while (true)
        {
            if (spawnCandidate[randomNum].GetComponent<EnemyStats>().HP <= 
                spawnCandidate[randomNum].GetComponent<EnemyStats>().HPMax-spawnEnemyHP) { break; }
            yield return new WaitForSeconds(0.1f);
        }
        */


        //END
        print("spawn enemy end!!!!!!!!!!!!!!!!!!!!!!!");
        //spawnCandidateParent.SetActive(false);
        //for(int i=0;i<spawnEnemies.Length;i++) { Destroy(spawnEnemies[i]); }
        //spawnEnemies = new GameObject[0];

        gameObject.transform.position = GameManager.instance.nowRoom.transform.position;
        yield return new WaitForSeconds(2f);


        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
        yield return null;
        
    }

    private void checkBite()
    {
        if(enemyStatus.hitTarget == null)
            return;

        //플레이어가 hit detection에 걸렸을 때 추가 효과
        if (patternNum == 2 && enemyStatus.hitTarget.CompareTag("Player"))
        {
            enemyStats.HP += 10;
        }
        else if(patternNum == 3 && enemyStatus.hitTarget.CompareTag("Player"))
        {
            enemyStatus.EnemyTarget.GetComponent<PlayerStats>().blind = blindTime;
        }
    }
}
