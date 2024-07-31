using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSloth : EnemyBasic
{
    //나태=아무것도 안함. 쉬어가는 텀. 근데 고민 뭘 조금 넣을지
    protected override void MovePattern()
    {
        enemyStatus.isRun = false;
    }



}
