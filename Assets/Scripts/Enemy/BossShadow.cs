using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShadow : EnemyBasic
{
    enum ShadowHitEffect { SightBlock, Dummy, Circle, None };

    public List<GameObject> dummies = new List<GameObject>();
    public List<GameObject> circles = new List<GameObject>();

    [SerializeField]
    private int patternNum=0;
    float time = 0;
    bool isDummiesAllDie;
    Renderer floorRenderer;

    protected override void Start()
    {
        base.Start();
        enemyStatus.isAttackReady = true;
        isDummiesAllDie = false;
        floorRenderer = GameManager.instance.nowRoom.GetComponent<Renderer>();
    }

    protected override void Update()
    {
        if (patternNum == 3 && !isDummiesAllDie)
        {  checkDummy(); }
        base.Update();
    }

    protected override void AttackPattern()
    {
        patternNum++;
        switch (patternNum)
        {
            case 1:
                enemyStatus.attackCoroutine = StartCoroutine(sightBlock_());
                break;
            case 2:
                enemyStatus.attackCoroutine = StartCoroutine(illusion_());
                break;
            case 3:
                enemyStatus.attackCoroutine = StartCoroutine(spawnDummy_());
                break;
            case 4:
                enemyStatus.attackCoroutine = StartCoroutine(darkSpawn_());
                break;
            default:
                patternNum = 0;
                break;

        }
    }


    IEnumerator hpRecovery_() 
    {
        if (stats.HP < 30)
        {
            for(int i=0;i<5;i++) 
            {
                stats.HP += 1;
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(30f);
        }
        yield return new WaitForSeconds(0.01f);
    }

    

    IEnumerator sightBlock_() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        print("sight block_shadow");

        hitEffects[(int)ShadowHitEffect.SightBlock].SetActive(true);

        float shotTime = 0;

        // 시야 차단이 좀 더 부드럽게 움직이도록 조정
        while (time < 10f)
        {
            if(shotTime > 0.5f) { shot(); shotTime = 0f; }
            hitEffects[(int)ShadowHitEffect.SightBlock].transform.position=enemyStatus.enemyTarget.transform.position;
            yield return null;
            time += Time.deltaTime;
            shotTime += Time.deltaTime;
        }
        time = 0;
        hitEffects[(int)ShadowHitEffect.SightBlock].SetActive(false);


        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(2f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator illusion_() 
    {
        enemyStatus.isAttackReady = false;

        print("illusion");

        //player cant move
        enemyStatus.enemyTarget.GetComponent<Player>().SetFlinch(3f);

        //fade out
        time = 0f;
        while (time < 1.5f)
        {
            sprite.color = new Color(1f, 1f, 1f, 1f - (time / 1.5f));
            yield return null;
            time += Time.deltaTime;
        }

        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;


        transform.position = getRandomPos();
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator spawnDummy_() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("spawnDummy");

        //Boss shadow invincible
        enemyStatus.isInvincible = true;

        hitEffects[(int)ShadowHitEffect.Dummy].SetActive(true);

        //spawn dummy
        for (int i = 0; i < dummies.Count; i++)
        {
            dummies[i].SetActive(true);

            //set dummy position
            dummies[i].transform.position = getRandomPos();
        }


        //reset dummy
        for (int i = 0; i < dummies.Count; i++)
        {
            dummies[i].GetComponent<EnemyStats>().HP 
                = dummies[i].GetComponent<EnemyStats>().HPMax;
        }

        isDummiesAllDie = false;
        while(isDummiesAllDie == false)
        {
            //attack
            Chase();
            shot();
            yield return new WaitForSeconds(3f);
        }

        hitEffects[(int)ShadowHitEffect.Dummy].SetActive(true);

        //Boss shadow vincible
        enemyStatus.isInvincible = false;

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

  

    IEnumerator darkSpawn_() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        hitEffects[(int)ShadowHitEffect.Circle].SetActive(true);

        //start
        for (int i = 0; i < circles.Count; i++)
        {
            
            circles[i].transform.position = enemyStatus.enemyTarget.position;
            yield return new WaitForSeconds(0.5f);
            circles[i].SetActive(true);
            yield return new WaitForSeconds(1f);
            //circles[i].transform.position = getRandomPos();
        }
        yield return new WaitForSeconds(2f);

        //finish
        for (int i = 0; i < circles.Count; i++)
        { circles[i].SetActive(false); }
        hitEffects[(int)ShadowHitEffect.Circle].SetActive(false);


        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(2f);
        enemyStatus.isAttackReady = true;

    }

    void  checkDummy() 
    {
        //check dummy
        for (int i = 0; i < dummies.Count; i++)
        {
            if (dummies[i].GetComponent<EnemyStats>().HP > 0)
            {
                return;
            }
        }
        isDummiesAllDie = true;
        
    }

    Vector3 getRandomPos() 
    {
        Bounds bounds = floorRenderer.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(randomX, randomY, 1);

    }
}
