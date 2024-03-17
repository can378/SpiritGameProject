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
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        status = GetComponent<EnemyStats>();
    }
    private void Update()
    {
        //5�ʵڿ��� ��Ȱ��ȭ
        time += Time.deltaTime;
        if (time >= 5f) { this.gameObject.SetActive(false); time = 0; }

        //�÷��̾������� �߻�
        Vector2 direction = transform.position- playerPos.position;
        transform.Translate(direction * status.defaultMoveSpeed * Time.deltaTime);    
    }

}
