using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWind : Skill
{
    [field: SerializeField] public int damage { get; private set; }     // ȸ�� �⺻ ���ط�
    [field: SerializeField] public int DPS { get; private set; }        // �ʴ� ���� �ӵ�
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public GameObject WheelWindEffect { get; private set; }
    [field: SerializeField] public GameObject instant { get; private set; }

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("SpinAttack");

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            MeleeWeapon meleeWeapon = player.mainWeaponController.mainWeapon.GetComponent<MeleeWeapon>();

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime + player.userData.skillCoolTime * skillDefalutCoolTime;

            // ���� = �÷��̾� ���� * ���� ����
            float attackRate = player.userData.playerAttackSpeed * meleeWeapon.attackSpeed;

            // ����
            yield return new WaitForSeconds(preDelay / attackRate);

            // ����� ��ġ�� ����
            instant = Instantiate(WheelWindEffect, user.transform.position, user.transform.rotation);
            instant.transform.parent = user.transform;


            // ���� ���� ����
            MultiHitDetection hitDetection = instant.GetComponent<MultiHitDetection>();

            // �Ӽ� = ���� �Ӽ�
            // ���ط� = (���� + �⺻ ���ط�) * �÷��̾� ���ݷ�
            // �˹� = ���� �˹�
            // ġȮ = �÷��̾� ġȮ
            // ġ�� = �÷��̾� ġ��
            hitDetection.SetMultiHitDetection(meleeWeapon.weaponAttribute,
             (meleeWeapon.damage + damage) * player.userData.playerPower,
             meleeWeapon.knockBack,
             player.userData.playerCritical,
             player.userData.playerCriticalDamage,
             meleeWeapon.weaponSize * size,
             (int)(DPS * attackRate)
             );
        }
    }

    public override void Exit(GameObject user)
    {
        StartCoroutine("AttackOut");
        
    }

    IEnumerator AttackOut()
    {
        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            MeleeWeapon meleeWeapon = player.mainWeaponController.mainWeapon.GetComponent<MeleeWeapon>();

            float attackRate = player.userData.playerAttackSpeed * meleeWeapon.attackSpeed;

            // rate ���� ����
            Destroy(instant);

            //�ĵ�
            yield return new WaitForSeconds(postDelay / attackRate);
        }
    }
}
