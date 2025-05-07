using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{

    public bool isTouchPlayer;
    public bool isBoss;                 // 보스 ?���?
    public bool isRun;                  // ?���? �? : 공격 ?�� ?�� ?��?���? ?��?��게서 �??���?
    public bool isTarget;               // ???겟을 ?��?��?���? ?��?���?
    public bool isTargetForced;         // �?�? ?��?��?��?�� 공격받으�? ???겟을 ?��?��?��?���?
    public float randomMove = 0;        // 무작?�� ?��?��
    public ObjectBasic EnemyTarget;       // ?��?�� ???�?
    public Vector2 targetDirVec;        // 공격 방향

    public Quaternion targetQuaternion 
    { 
        get {return Quaternion.Euler(0, 0, Mathf.Atan2(targetDirVec.y, targetDirVec.x)* Mathf.Rad2Deg - 90);}
    }
    public float targetDis;             // ?��과의 거리
    public Coroutine attackCoroutine;
}
