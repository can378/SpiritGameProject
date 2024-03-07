using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Armor : Equipment
{
    protected abstract void EquipArmor();

    protected abstract void UnEquipArmor();

    public override void Equip()
    {
        switch (equipmentOption)
        {
            case 0:
                break;
            case 1:
                Player.instance.stats.power += 0.25f;
                break;
            case 2:
                Player.instance.stats.attackSpeed += 0.25f;
                break;
            case 3:
                Player.instance.stats.skillPower += 0.5f;
                break;
            case 4:
                Player.instance.stats.skillCoolTime -= 0.25f;
                break;
            case 5:
                Player.instance.stats.critical += 0.25f;
                break;
            case 6:
                Player.instance.stats.criticalDamage += 0.25f;
                break;
            default:
                break;
        }
        EquipArmor();
    }

    public override void RandomOption()
    {
        Equip();
        equipmentOption = Random.Range(1, 6);
        UnEquip();
    }

    public override void UnEquip()
    {
        UnEquipArmor();

        switch (equipmentOption)
        {
            case 0:
                break;
            case 1:
                Player.instance.stats.power -= 0.25f;
                break;
            case 2:
                Player.instance.stats.attackSpeed -= 0.25f;
                break;
            case 3:
                Player.instance.stats.skillPower -= 0.5f;
                break;
            case 4:
                Player.instance.stats.skillCoolTime += 0.25f;
                break;
            case 5:
                Player.instance.stats.critical -= 0.25f;
                break;
            case 6:
                Player.instance.stats.criticalDamage -= 0.25f;
                break;
            default:
                break;
        }
    }


}
