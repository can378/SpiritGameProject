using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSloth : BossFace
{
    //����=�ƹ��͵� ����. ����� ��. �ٵ� ��� �� ���� ������
    protected override void MovePattern()
    {
        enemyStatus.isRun = false;
    }



}
