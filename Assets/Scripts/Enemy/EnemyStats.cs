using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    public string enemyName;
    public int DropCoin = 3;
    public int detectionDis;            // 탐지 거리            
    public int detectionKeepDis;        // 감지 후 유지 거리    // 음수 일 시 무한 유지
    public int maxAttackRange;          // 공격 사정거리        // 음수 일 시 무한 사정거리

    public int disableTime;             // bullet 등의 투사체가 몇초 후에 사라질지
}
