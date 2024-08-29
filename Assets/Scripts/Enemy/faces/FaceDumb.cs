using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDumb : BossFace
{

    //무지=막 돌아다님
    protected override void MovePattern()
    {
        if (nowAttack) { RandomMove(); }
    }

}
