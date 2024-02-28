using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{
    [field: SerializeField] public List<int> attackAttributes { get; private set; }
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public GameObject FireBallEffect { get; private set; }
    [field: SerializeField] public GameObject BurnDeBuff { get; private set; }

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
            Player player = user.GetComponent<Player>();
            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime + player.userData.skillCoolTime * skillDefalutCoolTime;

            // ���� = �÷��̾� ���� * ���� ����
            float attackRate = player.userData.playerAttackSpeed;

            // ����
            yield return new WaitForSeconds(preDelay / attackRate);

            GameObject instant = Instantiate(FireBallEffect, player.status.mousePos, Quaternion.identity);
            HitDetection hitDetection = instant.GetComponent<HitDetection>();
            /*
            ����ü = false
            ����� = -1
            �ٴ���Ʈ = false
            �ʴ� Ÿ�� Ƚ�� = -1 
            �Ӽ� = �� : 4
            ���ط� = ���ط� * �÷��̾� �ֹ���
            �˹� = �˹�
            ġȮ = 0
            ġ�� = 0
            ����� = ȭ��
            */
            hitDetection.SetHitDetection(false, -1, false, -1, attackAttributes, damage * player.userData.skillPower, knockBack,0,0, BurnDeBuff);

            Destroy(instant, rate);

            // �ĵ�
            yield return new WaitForSeconds(postDelay / attackRate);
        }
    }

    public override void Exit(GameObject user)
    {

    }
}
