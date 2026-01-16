using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IcicleSkill : SkillBase
{
    [field: SerializeField] IcicleSkillData ISData;

    GameObject simul;
    Vector3 simulVector;
    protected void Awake()
    {
        skillData = ISData;
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

            if(simul != null)
                Destroy(simul);
            simul = Instantiate(ISData.icicleSimulPrefab, player.CenterPivot.transform.position, Quaternion.identity);
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
            float angle = Mathf.Atan2(enemy.enemyStatus.EnemyTarget.transform.position.y - enemy.CenterPivot.transform.position.y, enemy.enemyStatus.EnemyTarget.transform.position.x - enemy.CenterPivot.transform.position.x) * Mathf.Rad2Deg;
            float timer = 0;

            if (simul != null)
                Destroy(simul);
            simul = Instantiate(ISData.icicleSimulPrefab, enemy.CenterPivot.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (timer <= ISData.maxHoldTime / 2 && enemy.enemyStatus.EnemyTarget != null  && enemy.enemyStatus.isAttack)
            {
                // ���߿� �� ���·� �ִ� ���� �����ϱ�
                // ���߿� �� ���·� �ִ� ���� ǥ���ϱ�
                angle = Mathf.Atan2(enemy.enemyStatus.EnemyTarget.transform.position.y - enemy.CenterPivot.transform.position.y, enemy.enemyStatus.EnemyTarget.transform.position.x - enemy.CenterPivot.transform.position.x) * Mathf.Rad2Deg;
                simulVector = enemy.enemyStatus.EnemyTarget.transform.position;
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
            GameObject instantProjectile = Instantiate(ISData.iciclePrefab, player.CenterPivot.transform.position, player.CenterPivot.transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * ISData.skillDefalutCoolTime;

            instantProjectile.transform.localScale = new Vector3(ISData.defaultSize, ISData.defaultSize, 0);
            instantProjectile.tag = "PlayerAttack";
            instantProjectile.layer = LayerMask.NameToLayer("PlayerAttack");

            Destroy(simul);
            
            hitDetection.SetProjectile_Ratio(0, ISData.defaultDamage, ISData.ratio, player.playerStats.SkillPower);
            //hitDetection.SetSEs(ISData.statusEffect);
            hitDetection.SetDisableTime(ISData.projectileTime);
            hitDetection.user = user;
            instantProjectile.transform.rotation = Quaternion.AngleAxis(player.playerStatus.mouseAngle - 90, Vector3.forward);  // ���� ����
            bulletRigid.velocity = (simulVector - player.CenterPivot.transform.position).normalized * 10 * ISData.projectileSpeed;  // �ӵ� ����
            
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            print(enemy);
            GameObject instantProjectile = Instantiate(ISData.iciclePrefab, enemy.CenterPivot.transform.position, enemy.CenterPivot.transform.rotation);
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(simulVector.y - enemy.CenterPivot.transform.position.y, simulVector.x - enemy.CenterPivot.transform.position.x) * Mathf.Rad2Deg;

            // ��Ÿ�� ����
            skillCoolTime = 5;

            instantProjectile.transform.localScale = new Vector3(ISData.defaultSize, ISData.defaultSize, 0);
            instantProjectile.tag = "EnemyAttack";
            instantProjectile.layer = LayerMask.NameToLayer("EnemyAttack");

            // ����Ʈ ����
            Destroy(simul);

            hitDetection.SetProjectile_Ratio(0, ISData.defaultDamage, ISData.ratio, enemy.stats.SkillPower, ISData.knockBack);
            //hitDetection.SetSEs(ISData.statusEffect);
            hitDetection.SetDisableTime(ISData.projectileTime);
            hitDetection.user = user;
            instantProjectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);  // ���� ����
            bulletRigid.velocity = (simulVector - enemy.CenterPivot.transform.position).normalized * 10 * ISData.projectileSpeed;  // �ӵ� ����
        }
    }
}
