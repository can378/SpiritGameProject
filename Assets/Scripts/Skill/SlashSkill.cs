using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlashSkill : Skill
{
    // ���ط�
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;

    // �ִ� ��ȭ��
    [field: SerializeField] public float maxHoldPower { get; private set; }

    // �ʴ� Ÿ�� Ƚ��, �⺻ ũ��, �ӵ�, ��Ÿ�
    [field: SerializeField] int DPS;
    [field: SerializeField] float size;
    [field: SerializeField] float speed;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject slashEffect;
    [field: SerializeField] GameObject slashEffectSimul;

    //���� ǥ�ñ�
    float holdPower;
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
            float powerTimer = 0;

            player.stats.decreasedMoveSpeed += 0.5f;
            holdPower = 1f;

            simul = Instantiate(slashEffectSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;
            simul.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.playerStats.weapon].attackSize, 0);

            while (player.status.isSkillHold)
            {
                if (holdPower < maxHoldPower && powerTimer > 0.1f)
                {
                    holdPower += 0.025f;
                    simul.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.playerStats.weapon].attackSize, 0);
                    powerTimer = 0;
                }
                simul.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 90, Vector3.forward);
                powerTimer += Time.deltaTime;
                yield return null;
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float angle = Mathf.Atan2(enemy.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
            float timer = 0;
            float powerTimer = 0;

            enemy.stats.decreasedMoveSpeed += 0.5f;
            holdPower = 1f;

            simul = Instantiate(slashEffectSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;
            simul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);

            while (timer < maxHoldTime / 2)
            {
                if (holdPower < maxHoldPower && powerTimer > 0.1f)
                {
                    holdPower += 0.025f;
                    simul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
                    powerTimer = 0;
                }
                angle = Mathf.Atan2(enemy.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
                simul.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                timer += Time.deltaTime;
                powerTimer += Time.deltaTime;
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
        Debug.Log("Slash");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            Weapon weapon = player.weaponController.weaponList[player.playerStats.weapon];
            GameObject instantProjectile = Instantiate(slashEffect, transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            float attackRate = weapon.SPA / player.playerStats.attackSpeed;                               // ���� 1ȸ�� �ɸ��� �ð�

            player.stats.decreasedMoveSpeed -= 0.5f;

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            //����Ʈ ����
            instantProjectile.transform.rotation = Quaternion.AngleAxis(player.status.mouseAngle - 90, Vector3.forward);  // ���� ����
            instantProjectile.transform.localScale = simul.transform.lossyScale;
            instantProjectile.tag = "PlayerAttack";
            instantProjectile.layer = LayerMask.NameToLayer("PlayerAttack");

            /*
            ����ü = true
            ����� = -1
            �ٴ���Ʈ = true
            �ʴ� Ÿ�� Ƚ�� = DPS / ���� 1ȸ�� �ɸ��� �ð�
            ���ط� = (�⺻ ���ط� + ���ݷ� * ���) * ��ȭ ��ġ
            �˹� = ���� �˹�
            ġȮ = �÷��̾� ġȮ
            ġ�� = �÷��̾� ġ��
            ����� = ���� �����̻�
            */
            hitDetection.SetHitDetection(true, -1, true, (int)((float)DPS / attackRate),
                (int)((defalutDamage + player.stats.attackPower * ratio) * holdPower),
                player.weaponController.weaponList[player.playerStats.weapon].knockBack * holdPower,
                player.playerStats.criticalChance,
                player.playerStats.criticalDamage,
                player.weaponController.weaponList[player.playerStats.weapon].statusEffect);
            bulletRigid.velocity = player.status.mouseDir * 10 * speed;

            Destroy(simul);
            Destroy(instantProjectile, time);  //��Ÿ� ����
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject instantProjectile = Instantiate(slashEffect, transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(enemy.enemyTarget.transform.position.y - user.transform.position.y, enemy.enemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;

            enemy.stats.decreasedMoveSpeed -= 0.5f;

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;

            // ����Ʈ ����
            instantProjectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);  // ���� ����
            instantProjectile.transform.localScale = simul.transform.lossyScale;
            instantProjectile.tag = "EnemyAttack";
            instantProjectile.layer = LayerMask.NameToLayer("EnemyAttack");
            /*
            ����ü = true
            ����� = -1
            �ٴ���Ʈ = true
            �ʴ� Ÿ�� Ƚ�� = DPS
            ���ط� = (�⺻ ���ط� + ���ݷ� * ���) * ��ȭ ��ġ
            �˹� = �⺻ �˹�
            ġȮ = 0
            ġ�� = 0
            ����� = 0
            */
            hitDetection.SetHitDetection(true, -1, true, DPS,
                (int)((defalutDamage + enemy.stats.attackPower * ratio) * holdPower),
                1 * holdPower,
                0,
                0,
                null);
            bulletRigid.velocity = (enemy.enemyTarget.transform.position - transform.position).normalized * 10 * speed;  // �ӵ� ����

            Destroy(simul);
            Destroy(instantProjectile, time);  //��Ÿ� ����
        }
    }
}
