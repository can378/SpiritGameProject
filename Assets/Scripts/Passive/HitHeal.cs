using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/HitHeal")]
public class HitHeal : PassiveData
{
    // �÷��̾ ���� ���� �� variation ��ŭ ȸ��
    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User)
    {
        if (_User == null || !_User.status.hitTarget)
            return;

        if (_User.status.hitTarget.gameObject.tag == "Wall" || _User.status.hitTarget.gameObject.tag == "Door" || _User.status.hitTarget.gameObject.tag == "ShabbyWall")
            return;

        _User.Damaged(-variation);
    }

    public override void Apply(ObjectBasic _User)
    {

    }

    // Update is called once per frame
    public override void Remove(ObjectBasic target)
    {

    }
}
