using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainWeaponType { Melee, Shot }

public class MainWeapon : Equipments
{
    [field: SerializeField] public MainWeaponType weaponType {get; private set;}
    [field: SerializeField] public int attackType { get; private set; }             //�ٰŸ� : 0 - �ֵθ���, 1 - ���, 2 - ��Ÿ
                                                                                    //���Ÿ� : 0 - ��, 1 - Ȱ, 2 - ������, 3 - ���� ����
    // 0 : ���Ӽ�, 1 : ����, 2 : Ÿ��, 3 : ����, 4 : ȭ��, 5 : �ñ�, 6 : ����, 7 : ��, 8 : ����, 9 : �ż�, 10 : ��� 
    [field: SerializeField] public List<int> attackAttribute { get; private set; }        
    
    [field: SerializeField] public bool isMultiHit { get; private set; }            // �ٴ���Ʈ ����
    [field: SerializeField] public int DPS { get; private set; }                    // �ʴ� Ÿ�� Ƚ�� �ʿ� ���� �� ����
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public float attackSpeed { get; private set; }          // ���ݼӵ�

    [field: SerializeField] public float preDelay { get; private set; }             // ��������
    [field: SerializeField] public float rate { get; private set; }                 // ���� �ð�
    [field: SerializeField] public float postDelay { get; private set; }            // ��� �ð�

    [field: SerializeField] public int maxAmmo { get; private set; }             // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public int ammo { get; private set; }                // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public float reloadTime { get; private set; }

    [field: SerializeField] public GameObject deBuff { get; private set; }
    

    public override void Equip()
    {
        switch(equipmentsOption)
        {
            case 0:
                break;
            case 1:
                Player.instance.stats.power += 0.5f;
                break;
            case 2:
                Player.instance.stats.attackSpeed += 0.5f;
                break;
            case 3:
                Player.instance.stats.skillPower += 1f;
                break;
            case 4:
                Player.instance.stats.skillCoolTime -= 0.5f;
                break;
            case 5:
                Player.instance.stats.critical += 0.5f;
                break;
            case 6:
                Player.instance.stats.criticalDamage += 0.5f;
                break;
            default:
                break;
        }
    }

    public override void RandomOption()
    {
        Equip();
        equipmentsOption = Random.Range(1,6);
        UnEquip();
    }

    public override void UnEquip()
    {
        switch (equipmentsOption)
        {
            case 0:
                break;
            case 1:
                Player.instance.stats.power -= 0.5f;
                break;
            case 2:
                Player.instance.stats.attackSpeed -= 0.5f;
                break;
            case 3:
                Player.instance.stats.skillPower -= 1f;
                break;
            case 4:
                Player.instance.stats.skillCoolTime += 0.5f;
                break;
            case 5:
                Player.instance.stats.critical -= 0.5f;
                break;
            case 6:
                Player.instance.stats.criticalDamage -= 0.5f;
                break;
            default:
                break;
        }
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

}
