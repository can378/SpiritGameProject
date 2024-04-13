using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : Skill
{
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public float time { get; private set; }
    [field: SerializeField] public GameObject spinEffect { get; private set; }
    
    [field: SerializeField] public float maxHoldPower { get; private set; }
    [field: SerializeField] public float holdPower { get; private set; }

    Coroutine HoldCoroutine;

    public override void Enter(GameObject user)
    {
        this.user = user;
        HoldCoroutine = StartCoroutine(Hold());
    }

    IEnumerator Hold()
    {
        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            player.stats.decreasedMoveSpeed += 0.5f;
            holdPower = 1f;
            while (holdPower < maxHoldPower)
            {
                yield return new WaitForSeconds(0.1f);
                holdPower += 0.05f;
            } 
        }
    }

    public override void Exit(GameObject user)
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
