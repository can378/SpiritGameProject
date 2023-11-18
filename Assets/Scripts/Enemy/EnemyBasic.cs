using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{
    //<공통>
    //외형, 이름, 스폰 위치
    //피, 공격(공격력, 공격속도,공격 범위, 공격 사거리), 이동속도, 특수 특성

    public Transform enemyTarget;
    public string name;
    public Sprite sprite;
    public Vector2 spawnPosition;
    public int health;
    public float speed;
    public int attack;
    

    GameObject enemy;

    private void Start()
    {
        enemy = GameObject.Find("Enemy");

    }

    private void Update()
    {

        //이동 구현? 장애물 피하거나 피하지 않고 이동

        //애니메이터
        if (health < 1f) { EnemyDead(); }
        else 
        {
            //살아있음 = 가만히 / 쫓아가기 / 탐색(숨어있을때)
            //공격하기 / 도망가기 / 특수행동

        }


    }

    public void EnemyDead() 
    {
        Debug.Log(this.gameObject.name + "는 죽었다.");
        //죽는 모션
        //아이템 떨구기
        //몬스터에 따라 특수 행동?
        //이 게임 오브젝트 파괴?
    
    }

    //플레이어와 닿으면 데미지 넣는다
    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            print("플레이어와 접촉. 요괴가 데미지를 넣는다.");
        }
    }*/

    void Chase()
    {
        Vector2 direction = enemyTarget.transform.position - transform.position;
        transform.Translate(direction * speed * Time.deltaTime);

    }
}
