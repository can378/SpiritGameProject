using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlashSkill : SkillBase
{
    [field: SerializeField] SlashSkillData SSData;

    //���� ǥ�ñ�
    float holdPower;
    GameObject simul;
    protected void Awake()
    {
        skillData = SSData;
    }

    public override void Enter(ObjectBasic user)
    {
        this.user = user;
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            PlayerWeapon weapon = player.playerStats.weapon;
            float powerTimer = 0;

            player.stats.MoveSpeed.DecreasedValue += 0.5f;
            holdPower = 1f;

            simul = Instantiate(SSData.slashSimulPrefab, player.CenterPivot.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;
            simul.transform.localScale = new Vector3(holdPower * SSData.effectSize * weapon.GetAttackSize(), holdPower * SSData.effectSize * weapon.GetAttackSize(), 0);

            while (player.playerStatus.isSkillHold)
            {
                if (holdPower < SSData.maxHoldPower && powerTimer > 0.1f)
                {
                    holdPower += 0.025f;
                    simul.transform.localScale = new Vector3(holdPower * SSData.effectSize * weapon.GetAttackSize(), holdPower * SSData.effectSize * weapon.GetAttackSize(), 0);
                    powerTimer = 0;
                }
                simul.transform.rotation = Quaternion.AngleAxis(player.playerStatus.mouseAngle - 90, Vector3.forward);
                powerTimer += Time.deltaTime;
                yield return null;
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float angle = Mathf.Atan2(enemy.enemyStatus.EnemyTarget.transform.position.y - enemy.CenterPivot.transform.position.y, enemy.enemyStatus.EnemyTarget.transform.position.x - enemy.CenterPivot.transform.position.x) * Mathf.Rad2Deg;
            float timer = 0;
            float powerTimer = 0;

            enemy.stats.MoveSpeed.DecreasedValue += 0.5f;
            holdPower = 1f;

            simul = Instantiate(SSData.slashSimulPrefab, enemy.CenterPivot.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;
            simul.transform.localScale = new Vector3(holdPower * SSData.effectSize, holdPower * SSData.effectSize, 0);

            while (timer < SSData.maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                if (holdPower < SSData.maxHoldPower && powerTimer > 0.1f)
                {
                    holdPower += 0.025f;
                    simul.transform.localScale = new Vector3(holdPower * SSData.effectSize, holdPower * SSData.effectSize, 0);
                    powerTimer = 0;
                }
                angle = Mathf.Atan2(enemy.enemyStatus.EnemyTarget.transform.position.y - user.transform.position.y, enemy.enemyStatus.EnemyTarget.transform.position.x - user.transform.position.x) * Mathf.Rad2Deg;
                simul.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                timer += Time.deltaTime;
                powerTimer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void Cancle()
    {
        StopCoroutine(Simulation());
        // �ù� ����
        // �̵� �ӵ� ȸ��
        Destroy(simul);
        user.GetComponent<Stats>().MoveSpeed.DecreasedValue -= 0.5f;
    }

    public override void Exit()
    {
        StopCoroutine(Simulation());
        Fire();
    }

    void Fire()
    {
        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            PlayerWeapon weapon = player.playerStats.weapon;
            GameObject instantProjectile = Instantiate(SSData.slashPrefab, player.CenterPivot.transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[weapon.weaponData.weaponType.ToString()];

            float attackRate = animationInfo.GetSPA() / player.playerStats.attackSpeed;                               // ���� 1ȸ�� �ɸ��� �ð�

            player.stats.MoveSpeed.DecreasedValue -= 0.5f;

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * SSData.skillDefalutCoolTime;

            //����Ʈ ����
            instantProjectile.transform.rotation = Quaternion.AngleAxis(player.playerStatus.mouseAngle - 90, Vector3.forward);  // ���� ����
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
            hitDetection.SetHit_Ratio(
                SSData.defaultDamage * holdPower, SSData.ratio * holdPower, player.stats.AttackPower,
                weapon.GetKnockBack()* holdPower,
                player.playerStats.CriticalChance,
                player.playerStats.CriticalDamage);
            hitDetection.SetMultiHit(true, (int)((float)SSData.DPS / attackRate));
            //hitDetection.SetSE((int)player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;
            bulletRigid.velocity = player.playerStatus.mouseDir * 10 * SSData.projectileSpeed;

            Destroy(simul);
            Destroy(instantProjectile, SSData.projectileTime);  //��Ÿ� ����
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject instantProjectile = Instantiate(SSData.slashPrefab, enemy.CenterPivot.transform.position, transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(enemy.enemyStatus.EnemyTarget.transform.position.y - enemy.CenterPivot.transform.position.y, enemy.enemyStatus.EnemyTarget.transform.position.x - enemy.CenterPivot.transform.position.x) * Mathf.Rad2Deg;

            enemy.stats.MoveSpeed.DecreasedValue -= 0.5f;

            // ��Ÿ�� ����
            skillCoolTime = 5;

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
            hitDetection.SetHit_Ratio(SSData.defaultDamage, SSData.ratio, enemy.stats.AttackPower, 1 * holdPower);
            hitDetection.SetMultiHit(true, SSData.DPS);
            hitDetection.user = user;
            bulletRigid.velocity = (enemy.enemyStatus.EnemyTarget.transform.position - enemy.CenterPivot.transform.position).normalized * 10 * SSData.projectileSpeed;  // �ӵ� ����

            Destroy(simul);
            Destroy(instantProjectile, SSData.projectileTime);  //��Ÿ� ����
        }
    }
}
