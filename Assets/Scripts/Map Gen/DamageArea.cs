using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : CurseArea
{
    [SerializeField] float Damaged;
    protected override void CurseEffect(ObjectBasic _Object)
    {
        _Object.Damaged(Damaged);
    }
}
