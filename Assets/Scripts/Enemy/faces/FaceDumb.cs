using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDumb : BossFace
{

    //����=�� ���ƴٴ�
    protected override void MovePattern()
    {
        if (nowAttack) { RandomMove(); }
    }

}
