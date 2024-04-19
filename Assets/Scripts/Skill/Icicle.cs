using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : Skill
{
    // ���ط�
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;

    // ũ��, �˹�, �ӵ�, ����Ʈ �����ð�, ����Ʈ, �����̻�
    [field: SerializeField] float size;
    [field: SerializeField] float knockBack;
    [field: SerializeField] float speed;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject icicleEffect;
    [field: SerializeField] int[] statusEffect;

    //���� ǥ�ñ�
    GameObject simul;

    public override void Enter(GameObject user)
    {
        this.user = user;
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            simul = Instantiate(GameData.instance.simulEffect[2], user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (player.status.isSkillHold)
            {
                // ���߿� �� ���·� �ִ� ���� �����ϱ�
                // ���߿� �� ���·� �ִ� ���� ǥ���ϱ�
                simul.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 90, Vector3.forward);
                yield return null;
            }
        }
    }

    public override void Exit()
    {
        StopCoroutine(Simulation());
        Fire();
    }

    void Fire()
    {
        Debug.Log("Icicle");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            // ����Ʈ ����
            GameObject instantProjectile = Instantiate(icicleEffect, transform.position, transform.rotation);
            Destroy(simul);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            /*
            ����ü = true
            ����� = 0
            �ٴ���Ʈ = false
            �ʴ� Ÿ�� Ƚ�� = -1 
            ���ط� = ���ط� * �÷��̾� ����
            �˹� = �˹�
            ġȮ = 0
            ġ�� = 0
            ����� = ȭ��
            */
            hitDetection.SetHitDetection(true, 0, false, -1, defalutDamage + player.stats.skillPower * ratio, knockBack, 0, 0, statusEffect);
            instantProjectile.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 270, Vector3.forward);  // ���� ����
            bulletRigid.velocity = player.status.mouseDir * 10 * speed;  // �ӵ� ����
            Destroy(instantProjectile, time);  //��Ÿ� ����
        }
    }
}
