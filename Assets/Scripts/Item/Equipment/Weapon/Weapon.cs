using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �ø��� MELEE�� RANGE ������ ��
public enum WEAPON_TYPE
{
    NONE = -1,
    // �ٰŸ� 
    SWORD, SHORT_SWORD, LONG_SWORD, SCYTHE, HAMMER, GREAT_HAMMER, SPEAR, MELEE,
    // ���Ÿ�
    BOW, CROSS_BOW, RIFLE, HAND_GUN, THROW, RANGE,
    END
}

public class Weapon : Equipment
{
    [field: SerializeField] public WEAPON_TYPE weaponType {get; private set;}       
                                                                                    
    
    [field: SerializeField] public bool isMultiHit { get; private set; }            // �ٴ���Ʈ ����
    [field: SerializeField] public int DPS { get; private set; }                    // �ʴ� Ÿ�� Ƚ�� �ʿ� ���� �� ����
    [field: SerializeField] public float attackPower { get; private set; }
    [field: SerializeField] public float attackSize { get; private set; }       // ����, ����ü ũ��

    [field: SerializeField] public float knockBack { get; private set; }

    //[field: SerializeField] public float preDelay { get; private set; }             // ��������
    //[field: SerializeField] public float rate { get; private set; }                 // ���� �ð�
    //[field: SerializeField] public float postDelay { get; private set; }            // ��� �ð�
    //public float SPA { get { return preDelay + rate + postDelay; } }                // 1ȸ ���ݿ� �ɸ��� �ð�

    [field: SerializeField] public int maxAmmo { get; private set; }             // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public int ammo { get; private set; }                // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public float reloadTime { get; private set; }

    [field: SerializeField] public GameObject projectile {get; private set; }
    [field: SerializeField] public float projectileSpeed { get; private set; }      // ����ü �ӵ�
    [field: SerializeField] public float projectileTime { get; private set; }       // ����ü ���� �ð�
    [field: SerializeField] public int penetrations { get; private set; }           // ����ü ���� Ƚ��

    [field: SerializeField] public SE_TYPE statusEffect {get; private set;}


    public override void Equip(Player user)
    {
        this.user = user;
        Stats stats = user.GetComponent<Stats>();
        stats.AttackPower.AddValue += attackPower;
        if(statusEffect != SE_TYPE.NONE)
            user.AddEnchant_SE(statusEffect);
    }

    public override void UnEquip(Player user)
    {
        Stats stats = user.GetComponent<Stats>();
        stats.AttackPower.AddValue -= attackPower;
        this.user = null;
        if (statusEffect != SE_TYPE.NONE)
            user.RemoveEnchant_SE(statusEffect);
    }

    public void Reload()
    {
        if(maxAmmo < 0)
            return;
        ammo = maxAmmo;
    }

    public void ConsumeAmmo()
    {
        if (maxAmmo < 0)
            return;
        ammo--;
    }

    public int SwingEffectType
    {
        get {
            if((int)WEAPON_TYPE.NONE < (int)weaponType && (int)weaponType < (int)WEAPON_TYPE.HAMMER)
                return 0;
            else if ((int)WEAPON_TYPE.SCYTHE < (int)weaponType && (int)weaponType < (int)WEAPON_TYPE.SPEAR)
                return 2;
            else if (weaponType == WEAPON_TYPE.SPEAR)
                return 1;
            else
                return -1;
        }
    }
    
    public int ShotPosType
    {
        get
        {
            switch (weaponType)
            {
                case WEAPON_TYPE.BOW:
                    return 3;
                case WEAPON_TYPE.CROSS_BOW:
                    return 4;
                case WEAPON_TYPE.RIFLE:
                    return 5;
                case WEAPON_TYPE.HAND_GUN:
                    return 6;
                case WEAPON_TYPE.THROW:
                    return 7;
                default:
                    return -1;
            }
        }
    }

    public string TypeToKorean()
    {
        {
            switch (weaponType)
            {
                case WEAPON_TYPE.SWORD:
                    return "��";
                case WEAPON_TYPE.SHORT_SWORD:
                    return "�ܰ�";
                case WEAPON_TYPE.LONG_SWORD:
                    return "���";
                case WEAPON_TYPE.SCYTHE:
                    return "��";
                case WEAPON_TYPE.HAMMER:
                    return "�б�";
                case WEAPON_TYPE.GREAT_HAMMER:
                    return "���� �б�";
                case WEAPON_TYPE.SPEAR:
                    return "â";
                case WEAPON_TYPE.BOW:
                    return "Ȱ";
                case WEAPON_TYPE.CROSS_BOW:
                    return "���";
                case WEAPON_TYPE.HAND_GUN:
                    return "��";
                case WEAPON_TYPE.RIFLE:
                    return "����";
                case WEAPON_TYPE.THROW:
                    return "��ô";
                default:
                    return "???";
            }
        }
    }

}
