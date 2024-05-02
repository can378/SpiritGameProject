using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackSkill : Skill
{
    // ���ط�
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }

    // �ִ� ��ȭ��
    [field: SerializeField] public float maxHoldPower { get; private set; }

    // �⺻ ũ��, ����Ʈ �����ð�, ����Ʈ
    [field: SerializeField] float size;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject spinEffect;
    [field: SerializeField] GameObject spinEffectSimul;

    // ��ȭ ��ġ
    float holdPower;
    GameObject simul;

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

            simul = Instantiate(spinEffectSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (holdPower < maxHoldPower && player.status.isSkillHold)
            {
                holdPower += 0.05f;
                simul.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer;

            enemy.stats.decreasedMoveSpeed += 0.5f;

            simul = Instantiate(spinEffectSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            holdPower = 1f;
            timer = 0;

            while (holdPower < maxHoldPower && timer <= maxHoldTime / 2)
            {
                holdPower += 0.05f;
                simul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
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
            Weapon weapon = player.weaponController.weaponList[player.stats.weapon];
            GameObject effect = Instantiate(spinEffect, user.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
            float attackRate = weapon.SPA / player.stats.attackSpeed;

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            Destroy(simul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, 0);
            effect.tag = "PlayerAttack";

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
             player.weaponController.weaponList[player.stats.weapon].knockBack * 10 * holdPower, 
             player.stats.criticalChance, 
             player.stats.criticalDamage,
             player.weaponController.weaponList[player.stats.weapon].statusEffect);

            // rate ���� ����
            player.stats.decreasedMoveSpeed -= 0.5f;
            Destroy(effect, time * attackRate);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(spinEffect, user.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // ��Ÿ�� ����
            skillCoolTime =skillDefalutCoolTime;

            Destroy(simul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
            effect.tag = "EnemyAttack";

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
             10 * holdPower,
             0,
             0,
             null);

            // rate ���� ����
            enemy.stats.decreasedMoveSpeed -= 0.5f;
            Destroy(effect, time);
        }
    }

    
}
