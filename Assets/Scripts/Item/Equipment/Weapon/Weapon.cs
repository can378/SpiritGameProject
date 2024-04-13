using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    [field: SerializeField] public int weaponType {get; private set;}               //�ٰŸ� : 0 - ����, 1 - ���, 2 - �ֵθ���
                                                                                    //���Ÿ� : 10 - ��, 11 - Ȱ, 12 - ������, 13 - ���� ����    
    
    [field: SerializeField] public bool isMultiHit { get; private set; }            // �ٴ���Ʈ ����
    [field: SerializeField] public int DPS { get; private set; }                    // �ʴ� Ÿ�� Ƚ�� �ʿ� ���� �� ����
    [field: SerializeField] public float attackPower { get; private set; }
    [field: SerializeField] public float attackSize { get; private set; }       // ����, ����ü ũ��

    [field: SerializeField] public float knockBack { get; private set; }

    [field: SerializeField] public float preDelay { get; private set; }             // ��������
    [field: SerializeField] public float rate { get; private set; }                 // ���� �ð�
    [field: SerializeField] public float postDelay { get; private set; }            // ��� �ð�
    public float SPA { get { return preDelay + rate + postDelay; } }                // 1ȸ ���ݿ� �ɸ��� �ð�

    [field: SerializeField] public int maxAmmo { get; private set; }             // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public int ammo { get; private set; }                // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public float reloadTime { get; private set; }

    [field: SerializeField] public GameObject projectile {get; private set; }
    [field: SerializeField] public float projectileSpeed { get; private set; }      // ����ü �ӵ�
    [field: SerializeField] public float projectileTime { get; private set; }       // ����ü ���� �ð�
    [field: SerializeField] public int penetrations { get; private set; }           // ����ü ���� Ƚ��

    [field: SerializeField] public GameObject[] statusEffect {get; private set;}


    public override void Equip(Player target)
    {
        this.target = target;
        Stats stats = target.GetComponent<Stats>();
        stats.addAttackPower += attackPower;
    }

    public override void UnEquip(Player target)
    {
        Stats stats = target.GetComponent<Stats>();
        stats.addAttackPower -= attackPower;
        this.target = null;

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
