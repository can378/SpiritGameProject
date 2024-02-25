using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Armor : Equipments
{
    public abstract void EquipArmor();

    public abstract void UnEquipArmor();

    public override void Equip()
    {
        switch (equipmentsOption)
        {
            case 0:
                break;
            case 1:
                Player.instance.userData.playerPower += 0.25f;
                break;
            case 2:
                Player.instance.userData.playerAttackSpeed += 0.25f;
                break;
            case 3:
                Player.instance.userData.skillPower += 0.5f;
                break;
            case 4:
                Player.instance.userData.skillCoolTime -= 0.25f;
                break;
            case 5:
                Player.instance.userData.playerCritical += 0.25f;
                break;
            case 6:
                Player.instance.userData.playerCriticalDamage += 0.25f;
                break;
            default:
                break;
        }
        EquipArmor();
    }

    public override void RandomOption()
    {
        Equip();
        equipmentsOption = Random.Range(1, 6);
        UnEquip();
    }

    public override void UnEquip()
    {
        UnEquipArmor();

        switch (equipmentsOption)
        {
            case 0:
                break;
            case 1:
                Player.instance.userData.playerPower -= 0.25f;
                break;
            case 2:
                Player.instance.userData.playerAttackSpeed -= 0.25f;
                break;
            case 3:
                Player.instance.userData.skillPower -= 0.5f;
                break;
            case 4:
                Player.instance.userData.skillCoolTime += 0.25f;
                break;
            case 5:
                Player.instance.userData.playerCritical -= 0.25f;
                break;
            case 6:
                Player.instance.userData.playerCriticalDamage -= 0.25f;
                break;
            default:
                break;
        }
    }


}
