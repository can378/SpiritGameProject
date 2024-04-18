using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : Skill
{
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;
    [field: SerializeField] float size;
    [field: SerializeField] float knockBack;
    [field: SerializeField] float speed;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject icicleEffect;
    [field: SerializeField] int[] statusEffect;

    public override void Enter(GameObject user)
    {
        this.user = user;
    }

    public override void Exit()
    {
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
