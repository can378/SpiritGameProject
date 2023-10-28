using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{
    GameObject enemy;
    Transform enemyTransform;
    private void Start()
    {
        enemy= GameObject.Find("Enemy");
        enemyTransform = enemy.GetComponent<Transform>();
    }

    private void Update()
    {
        print(enemyTransform);
    }
}
