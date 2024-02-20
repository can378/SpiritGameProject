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

        if(user.tag == "Player")
        {
            MeleeWeapon meleeWeapon = Player.instance.mainWeaponController.mainWeapon.GetComponent<MeleeWeapon>();

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime + Player.instance.userData.skillCoolTime * skillDefalutCoolTime;

            // ���� = �÷��̾� ���� * ���� ����
            float attackRate = Player.instance.userData.playerAttackSpeed * meleeWeapon.attackSpeed;

            // ����
            yield return new WaitForSeconds(preDelay / attackRate);

            // ����� ��ġ�� ����
            GameObject instant = Instantiate(spinEffect, user.transform.position, user.transform.rotation);

            // ���� ���� ����
            HitDetection hitDetection = instant.GetComponent<HitDetection>();

            // ũ�� ����
            instant.transform.localScale = new Vector3(size * meleeWeapon.weaponSize, size * meleeWeapon.weaponSize, 0);

            // �Ӽ� = ���� �Ӽ�
            // ���ط� = (���� + �⺻ ���ط�) * �÷��̾� ���ݷ�
            // �˹� = ���� �˹�
            // ġȮ = �÷��̾� ġȮ
            // ġ�� = �÷��̾� ġ��
            hitDetection.SetHitDetection(meleeWeapon.weaponAttribute,
             (meleeWeapon.damage + damage) * Player.instance.userData.playerPower,
             meleeWeapon.knockBack,
             Player.instance.userData.playerCritical,
             Player.instance.userData.playerCriticalDamage
             );

            // rate ���� ����
            Destroy(instant, rate / attackRate);

            //�ĵ�
            yield return new WaitForSeconds(postDelay / attackRate);
        }
    }

    public override void Exit(GameObject user)
    {
        
    }
}
