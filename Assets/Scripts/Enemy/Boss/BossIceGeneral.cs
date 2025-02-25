using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossIceGeneral : EnemyBasic
{

    private int attackIndex;
    public GameObject iceBlast;


    protected override void AttackPattern()
    {
        StartCoroutine(IceGerneral());
    }


    IEnumerator IceGerneral() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        enemyStatus.targetDis =Vector2.Distance(enemyStatus.enemyTarget.transform.position,transform.position);


        if (enemyStatus.targetDis > 10f)
        {
            //shoot knife
            GameObject bullet = ObjectPoolManager.instance.Get("Bullet");
            bullet.transform.position = transform.position;
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            enemyStatus.targetDirVec = enemyStatus.enemyTarget.transform.position - transform.position;
            bulletRigid.AddForce(enemyStatus.targetDirVec.normalized * 5, ForceMode2D.Impulse);

            yield return new WaitForSeconds(2f);
        }
        else 
        {
            switch (attackIndex)
            {
                case 0:
                    //sword swing
                    for (int i = 0; i < 5; i++)
                    {
                        enemyStatus.targetDirVec=enemyStatus.enemyTarget.transform.position - transform.position;
                        rigid.AddForce(enemyStatus.targetDirVec*GetComponent<EnemyStats>().MoveSpeed.Value, ForceMode2D.Impulse);
                        print("sword swing!!");
                        yield return new WaitForSeconds(0.1f);
                    }
                    break;
                case 1:
                    //exuding chills
                    print("exuding chills");
                    iceBlast.SetActive(true);
                    yield return new WaitForSeconds(3f);
                    iceBlast.SetActive(false);
                    break;
                case 2:
                    //sword slash
                    print("sword slash");
                    yield return new WaitForSeconds(4f);
                    break;
                default: break;
            }


            attackIndex ++;
            if (attackIndex > 2) { attackIndex = 0; }

            yield return new WaitForSeconds(2f);
        }


        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }


}
