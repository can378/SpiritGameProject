using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : Skill
{
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public int size { get; private set; }
    [field: SerializeField] public GameObject spinEffect { get; private set; }

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("SpinAttack");

        Player player = user.GetComponent<Player>();
        MeleeWeapon meleeWeapon = player.mainWeaponController.mainWeapon.GetComponent<MeleeWeapon>();

        // ���� = �÷��̾� ���� * ���� ����
        float attackRate = player.userData.playerAttackSpeed * meleeWeapon.attackSpeed;

        yield return new WaitForSeconds(preDelay / attackRate);

        GameObject instant = Instantiate(spinEffect, user.transform.position,user.transform.rotation);

        if(user.tag == "Player")
        {
            HitDetection hitDetection = instant.GetComponent<HitDetection>();

            // ũ�� ����
            instant.transform.localScale = new Vector3(size * meleeWeapon.weaponSize, size * meleeWeapon.weaponSize,0);

            // �Ӽ� = ���� �Ӽ�
            // ���ط� = (���� + �⺻ ���ط�) * �÷��̾� ���ݷ�
            // �˹� = ���� �˹�
            // ġȮ = �÷��̾� ġȮ
            // ġ�� = �÷��̾� ġ��
            hitDetection.SetHitDetection(meleeWeapon.weaponAttribute,
             (meleeWeapon.damage + damage) * player.userData.playerPower,
             meleeWeapon.knockBack,
             player.userData.playerCritical,
             player.userData.playerCriticalDamage
             );
        }

        Destroy(instant, rate / attackRate);
        
        yield return new WaitForSeconds(postDelay / attackRate);

        StartCoroutine("CoolDown");

    }
}