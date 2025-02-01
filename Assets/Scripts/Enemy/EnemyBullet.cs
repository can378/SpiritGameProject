using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float time=0;
    private EnemyStats stats;
    Transform playerPos;
 
    private void Start()
    {
        playerPos = FindObj.instance.Player.transform;
        stats = GetComponent<EnemyStats>();
    }
    private void Update()
    {
        //5�ʵڿ��� ��Ȱ��ȭ
        time += Time.deltaTime;
        if (time >= 5f) { gameObject.SetActive(false); time = 0; }

        //�÷��̾������� �߻�
        Vector2 direction = playerPos.position- transform.position;
        transform.Translate(direction * stats.MoveSpeed.Value * Time.deltaTime);  
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player") || collision.CompareTag("Wall")) { gameObject.SetActive(false); }
    // }
}
