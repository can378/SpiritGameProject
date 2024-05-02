using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShadow : EnemyBasic
{
    public GameObject sightBlock;
    public List<GameObject> dummies = new List<GameObject>();
    public List<GameObject> circles = new List<GameObject>();

    private int patternNum;
    int time = 0;

    private void OnEnable()
    {
        patternNum = 0;
        StartNamedCoroutine("bossShadow",bossShadow());
        StartNamedCoroutine("hpRecovery_", hpRecovery_());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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
        StartCoroutine(hpRecovery_());
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
                StartCoroutine(bossShadow());
                break;
        
        }
        yield return null;
    
    }

    

    IEnumerator sightBlock_() 
    {
        sightBlock.SetActive(true);
        while (time < 100)
        {
            shot();
            sightBlock.transform.position=enemyTarget.transform.position;
            yield return new WaitForSeconds(0.1f);
            time++;
        }
        sightBlock.SetActive(false);
        StartCoroutine(bossShadow());
    }

    IEnumerator shot_() 
    {
        shot();
        yield return new WaitForSeconds(1f);

    }

    IEnumerator illusion_() 
    {
        //player cant move
        //?????????????????

        //fade out
        float alpha = 255f;
        while (alpha > 0)
        {
            sprite.color = new Color(255f, 255f, 255f, alpha);
            alpha--;
        }
        
        yield return null;
        StartCoroutine(bossShadow());

    }
    IEnumerator spawnDummy_() 
    {
        //Boss shadow invincible
        //?????



        //spawn dummy
        for (int i = 0; i < dummies.Count; i++)
        {
            dummies[i].SetActive(true);
        }

        bool isDummiesAllDie = false;
        while(isDummiesAllDie==false)
        {
            //attack
            Chase();
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
        //????????
        StartCoroutine(bossShadow());
    }

  

    IEnumerator darkSpawn_() 
    {
        for (int i = 0; i < circles.Count; i++)
        {
            circles[i].SetActive(true);
        }
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < circles.Count; i++)
        {
            circles[i].SetActive(false);
        }
        StartCoroutine(bossShadow());

    }

}
