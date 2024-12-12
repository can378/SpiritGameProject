using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackSkill : Skill
{
    // ���ط�
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;

    // �ִ� ��ȭ��
    [field: SerializeField] float maxHoldPower;

    // �⺻ ũ��, ����Ʈ �����ð�, ����Ʈ
    [field: SerializeField] float size;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject spinPrefab;
    [field: SerializeField] GameObject spinSimulPrefab;

    // ��ȭ ��ġ
    float holdPower;
    GameObject spinSimul;

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            player.stats.decreasedMoveSpeed += 0.5f;
            holdPower = 1f;

            spinSimul = Instantiate(spinSimulPrefab, user.gameObject.transform.position, Quaternion.identity);
            spinSimul.transform.parent = user.transform;

            while (holdPower < maxHoldPower && player.playerStatus.isSkillHold)
            {
                holdPower += 0.05f;
                spinSimul.transform.localScale = new Vector3(holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer;

            enemy.stats.decreasedMoveSpeed += 0.5f;

            spinSimul = Instantiate(spinSimulPrefab, user.gameObject.transform.position, Quaternion.identity);
            spinSimul.transform.parent = user.transform;

            holdPower = 1f;
            timer = 0;

            while (holdPower < maxHoldPower && timer <= maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                holdPower += 0.05f;
                spinSimul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        StopCoroutine(Simulation());
        user.GetComponent<Stats>().decreasedMoveSpeed -= 0.5f;
        Destroy(spinSimul);
    }

    public override void Exit()
    {
        StopCoroutine(Simulation());
        Attack();
    }

    void Attack()
    {
        Debug.Log("SpinAttack");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponList[player.playerStats.weapon];
            GameObject effect = Instantiate(spinPrefab, user.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
            float attackRate = weapon.SPA / player.playerStats.attackSpeed;

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, 0);
            effect.tag = "PlayerAttack";
            effect.layer = LayerMask.NameToLayer("PlayerAttack");
            /*
            ����ü = false
            ����� = -1
            �ٴ���Ʈ = false
            �ʴ� Ÿ�� Ƚ�� = -1 
            ���ط� = (�⺻ ���ط� + ���� ���ط�) * �÷��̾� ���ݷ�
            �˹� = ���� �˹�
            ġȮ = �÷��̾� ġȮ
            ġ�� = �÷��̾� ġ��
            ����� = ����
            */
            hitDetection.SetHitDetection(false, -1, false, -1,
             defalutDamage + player.stats.attackPower * ratio * holdPower,
             player.weaponList[player.playerStats.weapon].knockBack * 10 * holdPower, 
             player.playerStats.criticalChance, 
             player.playerStats.criticalDamage);
            hitDetection.SetSEs(player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;

            // rate ���� ����
            player.stats.decreasedMoveSpeed -= 0.5f;
            Destroy(effect, time * attackRate);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(spinPrefab, user.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // ��Ÿ�� ����
            skillCoolTime =skillDefalutCoolTime;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
            effect.tag = "EnemyAttack";
            effect.layer = LayerMask.NameToLayer("EnemyAttack");

            /*
            ����ü = false
            ����� = -1
            �ٴ���Ʈ = false
            �ʴ� Ÿ�� Ƚ�� = -1 
            ���ط� = (�⺻ ���ط� + ���� ���ط�) * �÷��̾� ���ݷ�
            �˹� = ���� �˹�
            ġȮ = �÷��̾� ġȮ
            ġ�� = �÷��̾� ġ��
            ����� = ����
            */
            hitDetection.SetHitDetection(false, -1, false, -1,
             defalutDamage + enemy.stats.attackPower * ratio * holdPower,
             10 * holdPower);
            hitDetection.user = user;

            // rate ���� ����
            enemy.stats.decreasedMoveSpeed -= 0.5f;
            Destroy(effect, time);
        }
    }

    
}
