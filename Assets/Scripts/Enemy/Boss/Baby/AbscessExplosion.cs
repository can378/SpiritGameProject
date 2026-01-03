using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbscessExplosion : CurseArea
{
    protected override void CurseEffect(ObjectBasic _Object)
    {
        if (_Object is Abscess myScript)
        {
            if(_Object.status.isDead)
                return;
            myScript.Explosion();
        }
    }
}
