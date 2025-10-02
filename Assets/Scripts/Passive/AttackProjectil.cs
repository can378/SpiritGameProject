using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/AttackProjectil")]

public class AttackProjectil : PassiveData
{
    // Åõ»çÃ¼°¡ ÆÄ±« ½Ã Æø¹ß
    public PROJECTILE_TYPE projectileType;

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic _User)
    {
        _User.AddEnchant_Projectile(projectileType);
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        _User.RemoveEnchant_Projectile(projectileType);
    }
}
