using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShadow : EnemyBasic
{
    public GameObject sightBlock;
    public List<GameObject> dummies = new List<GameObject>();
    public List<GameObject> circles = new List<GameObject>();

    private int patternNum=0;
    int time = 0;

    private void Start()
    {
        isAttackReady = true;
    }

    protected override void AttackPattern()
    {
        StartCoroutine(bossShadow());
        //StartCoroutine(hpRecovery_());
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


    IEnumerator bossShadow() 
    {

        
        patternNum++;
        
        switch (patternNum)
        {
            case 1:
                StartCoroutine(sightBlock_());
                break;
            case 2:
                StartCoroutine(illusion_());
                break;
            case 3:
                StartCoroutine(spawnDummy_());
                break;
            case 4:
                StartCoroutine(darkSpawn_());
                break;
            default:
                patternNum = 0;
                break;
        
        }
        yield return null;
    
    }

    

    IEnumerator sightBlock_() 
    {
        isAttack = true;
        isAttackReady = false;

        print("sight block_shadow");

        sightBlock.SetActive(true);
        while (time < 100)
        {
            if(time%10==0) { shot(); }
            sightBlock.transform.position=enemyTarget.transform.position;
            yield return new WaitForSeconds(0.1f);
            time++;
        }
        sightBlock.SetActive(false);
        
        
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator shot_() 
    {
        shot();
        yield return new WaitForSeconds(1f);

    }

    IEnumerator illusion_() 
    {
        isAttack = true;
        isAttackReady = false;
        
        //player cant move
        enemyTarget.GetComponent<Player>().isFlinch = true;
        //enemyTarget.GetComponent<Player>().moveVec = Vector3.zero;

        //fade out
        float alpha = 255f;
        while (alpha > 0)
        {
            sprite.color = new Color(255f, 255f, 255f, alpha);
            alpha--;
        }

        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

        enemyTarget.GetComponent<Player>().isFlinch = false;

    }

    IEnumerator spawnDummy_() 
    {
        isAttack = true;
        isAttackReady = false;
        print("spawnDummy");

        //Boss shadow invincible
        isInvincible = true;



        //spawn dummy
        for (int i = 0; i < dummies.Count; i++)
        {
            dummies[i].SetActive(true);
        }

        bool isDummiesAllDie = false;
        while(isDummiesAllDie==false)
        {
            //attack
            //Chase();
            shot();

            //check dummy
            for (int i = 0; i < dummies.Count; i++)
            {
                if (dummies[i].GetComponent<EnemyStats>().HP > 0)
                {
                    break;
                }
                if (i == dummies.Count - 1) 
                { isDummiesAllDie = true; }
            }
            yield return new WaitForSeconds(0.01f);
        }


        //Boss shadow vincible
        isInvincible = false;

        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

  

    IEnumerator darkSpawn_() 
    {
        isAttack = true;
        isAttackReady = false;

        print("dark spawn");
        for (int i = 0; i < circles.Count; i++)
        {
            circles[i].SetActive(true);
        }
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < circles.Count; i++)
        {
            circles[i].SetActive(false);
        }
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }

}
