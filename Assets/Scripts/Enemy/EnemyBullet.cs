using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float time=0;
    private EnemyStats status;
    Transform playerPos;

    void Awake()
    {
        playerPos=GameObject.FindWithTag("Player").GetComponent<Transform>();
        status = GetComponent<EnemyStats>();
    }
    private void Update()
    {
        //5초뒤에는 비활성화
        time += Time.deltaTime;
        if (time >= 5f) { this.gameObject.SetActive(false); time = 0; }

        //플레이어쪽으로 발사
        Vector2 direction = playerPos.position- transform.position;
        transform.Translate(direction * status.defaultMoveSpeed * Time.deltaTime);    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        { gameObject.SetActive(false); }
    }
}
