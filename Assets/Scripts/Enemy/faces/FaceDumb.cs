using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDumb : BossFace
{

    //����=�� ���ƴٴ�

    public GameObject attackArea;

    protected override void Update()
    {
        base.Update();
        attackArea.transform.localPosition = new Vector3(0, 0, 0);
    }


    protected override void MovePattern()
    {
        print("dumb");
        Chase();
        
    }

}
