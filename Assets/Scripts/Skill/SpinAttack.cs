using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : Skill
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

    float holdPower;
    GameObject simul;
    Coroutine HoldCoroutine;

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        HoldCoroutine = StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponController.weaponList[player.stats.weapon];
            player.stats.decreasedMoveSpeed += 0.5f;
            holdPower = 1f;

            simul = Instantiate(GameData.instance.simulEffect[1], user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (holdPower < maxHoldPower && player.status.isSkillHold)
            {
                holdPower += 0.05f;
                simul.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void Exit()
    {
        StopCoroutine(HoldCoroutine);
        Attack();
    }

    void Attack()
    {
        Debug.Log("SpinAttack");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponController.weaponList[player.stats.weapon];

            player.stats.decreasedMoveSpeed -= 0.5f;

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            // ���ݿ� �ɸ��� �ð� = ���� 1ȸ�� �ɸ��� �ð� / �÷��̾� ���ݼӵ�
            // ���� ���� ����
            float attackRate = weapon.SPA / player.stats.attackSpeed;

            // ����� ��ġ�� ����
            GameObject effect = Instantiate(spinEffect, user.transform.position, user.transform.rotation);
            effect.transform.parent = user.transform;
            Destroy(simul);

            // ���� ���� ����
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // ũ�� ����
            
            effect.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, 0);
            
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
            Destroy(effect, time * attackRate);
        }
    }

    
}
