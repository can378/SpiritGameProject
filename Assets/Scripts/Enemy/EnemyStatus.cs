using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{

    public bool isTouchPlayer;
    public bool isRun;                  // 도망 중 : 공격 할 수 없으며 적에게서 멀어짐
    public float randomMove = 0;        // 무작위 이동
    public Transform enemyTarget;       // 현재 타겟
    public Vector2 targetDirVec;        // 공격 방향

    public Quaternion targetQuaternion 
    { 
        get {return Quaternion.Euler(0, 0, Mathf.Atan2(targetDirVec.y, targetDirVec.x)* Mathf.Rad2Deg - 90);}
    }
    public float targetDis;             // 적과의 거리
    public Coroutine attackCoroutine;
}
