using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IcicleSkill : Skill
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
    [field: SerializeField] GameObject fireSimul;
    [field: SerializeField] int[] statusEffect;

    //���� ǥ�ñ�
    GameObject simul;
    Vector3 simulVector;

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

            if(simul != null)
                Destroy(simul);
            simul = Instantiate(fireSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (player.playerStatus.isSkillHold)
            {
                // ���߿� �� ���·� �ִ� ���� �����ϱ�
                // ���߿� �� ���·� �ִ� ���� ǥ���ϱ�
                simulVector = player.playerStatus.mousePos;
                simul.transform.rotation = Quaternion.AngleAxis(player.playerStatus.mouseAngle - 90, Vector3.forward);
                yield return null;
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float angle = Mathf.Atan2(enemy.enemyStatus.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyStatus.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
            float timer = 0;

            if (simul != null)
                Destroy(simul);
            simul = Instantiate(fireSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (timer <= maxHoldTime / 2 && enemy.enemyStatus.enemyTarget != null  && enemy.enemyStatus.isAttack)
            {
                // ���߿� �� ���·� �ִ� ���� �����ϱ�
                // ���߿� �� ���·� �ִ� ���� ǥ���ϱ�
                angle = Mathf.Atan2(enemy.enemyStatus.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyStatus.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
                simulVector = enemy.enemyStatus.enemyTarget.transform.position;
                simul.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void Cancle()
    {
        StopCoroutine(Simulation());
        Destroy(simul);
    }

    public override void Exit()
    {
        base.Cancle();
        StopCoroutine(Simulation());
        Fire();
    }

    void Fire()
    {
        Debug.Log("Icicle");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            GameObject instantProjectile = Instantiate(icicleEffect, transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            instantProjectile.transform.localScale = new Vector3(size, size, 0);
            instantProjectile.tag = "PlayerAttack";
            instantProjectile.layer = LayerMask.NameToLayer("PlayerAttack");

            Destroy(simul);
            
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
            hitDetection.SetHitDetection(true, 0, false, -1, defalutDamage + player.playerStats.skillPower * ratio, knockBack, 0, 0, statusEffect);
            hitDetection.user = user;
            instantProjectile.transform.rotation = Quaternion.AngleAxis(player.playerStatus.mouseAngle - 90, Vector3.forward);  // ���� ����
            bulletRigid.velocity = (simulVector - user.transform.position).normalized * 10 * speed;  // �ӵ� ����
            
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject instantProjectile = Instantiate(icicleEffect, transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(simulVector.y - user.transform.position.y, simulVector.x - user.transform.position.x) * Mathf.Rad2Deg;

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;

            instantProjectile.transform.localScale = new Vector3(size, size,0);
            instantProjectile.tag = "EnemyAttack";
            instantProjectile.layer = LayerMask.NameToLayer("EnemyAttack");

            // ����Ʈ ����
            Destroy(simul);


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
            hitDetection.SetHitDetection(true, 0, false, -1, defalutDamage + enemy.stats.attackPower * ratio, knockBack, 0, 0, statusEffect);
            hitDetection.user = user;
            instantProjectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);  // ���� ����
            bulletRigid.velocity = (simulVector - user.transform.position).normalized * 10 * speed;  // �ӵ� ����
            Destroy(instantProjectile, time);  //��Ÿ� ����
        }
    }
}
