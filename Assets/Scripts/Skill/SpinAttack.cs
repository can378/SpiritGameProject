using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : Skill
{
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public float size { get; private set; }
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
            Player player = this.user.GetComponent<Player>();
            MeleeWeapon meleeWeapon = player.mainWeaponController.mainWeapon.GetComponent<MeleeWeapon>();

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime + player.userData.skillCoolTime * skillDefalutCoolTime;

            // ���� = �÷��̾� ���� * ���� ����
            float attackRate = player.userData.playerAttackSpeed * meleeWeapon.attackSpeed;

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
            hitDetection.SetHitDetection(meleeWeapon.attackAttribute,
             (meleeWeapon.damage + damage) * player.userData.playerPower,
             meleeWeapon.knockBack,
             player.userData.playerCritical,
             player.userData.playerCriticalDamage
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
