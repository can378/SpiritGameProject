using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearExplosion : CurseArea
{
    protected override void CurseEffect(ObjectBasic _Object)
    {
        if (_Object is Tear myScript)
        {
            if(_Object.status.isDead)
                return;
            myScript.Explosion();
        }
    }
}
