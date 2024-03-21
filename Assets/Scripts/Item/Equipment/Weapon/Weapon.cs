using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    [field: SerializeField] public int weaponType {get; private set;}               //�ٰŸ� : 0 - ����, 1 - ���, 2 - �ֵθ���
                                                                                    //���Ÿ� : 10 - ��, 11 - Ȱ, 12 - ������, 13 - ���� ����
    // 0 : ���Ӽ�, 1 : ����, 2 : Ÿ��, 3 : ����, 4 : ȭ��, 5 : �ñ�, 6 : ����, 7 : ��, 8 : ����, 9 : �ż�, 10 : ��� 
    [field: SerializeField] public List<int> attackAttribute { get; private set; }        
    
    [field: SerializeField] public bool isMultiHit { get; private set; }            // �ٴ���Ʈ ����
    [field: SerializeField] public int DPS { get; private set; }                    // �ʴ� Ÿ�� Ƚ�� �ʿ� ���� �� ����
    [field: SerializeField] public float attackPower { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public float attackSpeed { get; private set; }          // ���ݼӵ�

    [field: SerializeField] public float preDelay { get; private set; }             // ��������
    [field: SerializeField] public float rate { get; private set; }                 // ���� �ð�
    [field: SerializeField] public float postDelay { get; private set; }            // ��� �ð�

    [field: SerializeField] public GameObject deBuff { get; private set; }

    [field: SerializeField] public float attackSize { get; private set; }       // ����, ����ü ũ��

    [field: SerializeField] public int maxAmmo { get; private set; }             // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public int ammo { get; private set; }                // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public float reloadTime { get; private set; }

    [field: SerializeField] public GameObject projectile {get; private set; }
    [field: SerializeField] public float projectileSpeed { get; private set; }      // ����ü �ӵ�
    [field: SerializeField] public float projectileTime { get; private set; }       // ����ü ���� �ð�
    [field: SerializeField] public int penetrations { get; private set; }           // ����ü ���� Ƚ��


    public override void Equip()
    {
        Player.instance.stats.addAttackPower = attackPower;
    }

    public override void UnEquip()
    {
        Player.instance.stats.addAttackPower -= attackPower;
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
