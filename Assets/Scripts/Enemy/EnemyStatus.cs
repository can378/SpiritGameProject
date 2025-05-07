using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{

    public bool isTouchPlayer;
    public bool isBoss;                 // ë³´ìŠ¤ ?—¬ë¶?
    public bool isRun;                  // ?„ë§? ì¤? : ê³µê²© ?•  ?ˆ˜ ?—†?œ¼ë©? ? ?—ê²Œì„œ ë©??–´ì§?
    public bool isTarget;               // ???ê²Ÿì„ ?¸?‹?•˜ê³? ?ˆ?Š”ì§?
    public bool isTargetForced;         // ë©?ë¦? ?ˆ?”?¼?„ ê³µê²©ë°›ìœ¼ë©? ???ê²Ÿì„ ?¸?‹?•˜?Š”ì§?
    public float randomMove = 0;        // ë¬´ì‘?œ„ ?´?™
    public ObjectBasic EnemyTarget;       // ?˜„?¬ ???ê²?
    public Vector2 targetDirVec;        // ê³µê²© ë°©í–¥

    public Quaternion targetQuaternion 
    { 
        get {return Quaternion.Euler(0, 0, Mathf.Atan2(targetDirVec.y, targetDirVec.x)* Mathf.Rad2Deg - 90);}
    }
    public float targetDis;             // ? ê³¼ì˜ ê±°ë¦¬
    public Coroutine attackCoroutine;
}
