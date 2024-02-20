using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public int size { get; private set; }
    [field: SerializeField] public int knockBack { get; private set; }
    [field: SerializeField] public GameObject FireBallEffect { get; private set; }

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        Debug.Log("FireBall");

        if (user.tag == "Player")
        {
            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime + Player.instance.userData.skillCoolTime * skillDefalutCoolTime;

            // ���� = �÷��̾� ���� * ���� ����
            float attackRate = Player.instance.userData.playerAttackSpeed;

            yield return new WaitForSeconds(preDelay / attackRate);

            GameObject instant = Instantiate(FireBallEffect, Player.instance.status.mousePos, Quaternion.identity);
            HitDetection hitDetection = instant.GetComponent<HitDetection>();
            // �Ӽ� = ��
            // ���ط� = ���ط� * �÷��̾� ���ݷ�
            // �˹� = �˹�
            // ġȮ = 0
            // ġ�� = 0
            hitDetection.SetHitDetection(WeaponAttribute.Fire,
             damage * Player.instance.userData.skillPower,
             knockBack,
             0,
             0
             );

            Destroy(instant, rate);

            yield return new WaitForSeconds(postDelay / attackRate);
        }
    }

    public override void Exit(GameObject user)
    {

    }
}
