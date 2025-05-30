using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{

    public bool isTouchPlayer;
    public bool isBoss;                 // λ³΄μ€ ?¬λΆ?
    public bool isRun;                  // ?λ§? μ€? : κ³΅κ²© ?  ? ??Όλ©? ? ?κ²μ λ©??΄μ§?
    public bool isTarget;               // ???κ²μ ?Έ??κ³? ??μ§?
    public bool isTargetForced;         // λ©?λ¦? ???Ό? κ³΅κ²©λ°μΌλ©? ???κ²μ ?Έ???μ§?
    public float randomMove = 0;        // λ¬΄μ? ?΄?
    public ObjectBasic EnemyTarget;       // ??¬ ???κ²?
    public Vector2 targetDirVec;        // κ³΅κ²© λ°©ν₯

    public Quaternion targetQuaternion 
    { 
        get {return Quaternion.Euler(0, 0, Mathf.Atan2(targetDirVec.y, targetDirVec.x)* Mathf.Rad2Deg - 90);}
    }
    public float targetDis;             // ? κ³Όμ κ±°λ¦¬
    public Coroutine attackCoroutine;
}
