using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWind : Skill
{
    [field: SerializeField] public int damage { get; private set; }     // ȸ�� �⺻ ���ط�
    [field: SerializeField] public int DPS { get; private set; }        // �ʴ� ���� �ӵ�
    [field: SerializeField] public float size { get; private set; }     // ����Ʈ ũ��
    [field: SerializeField] public GameObject WheelWindEffect { get; private set; }     //������ prefep ����Ʈ
    GameObject effect;      // ����Ʈ

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("WheelWind");

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.stats.decreasedSkillCoolTime) * skillDefalutCoolTime;

            // ���� = �÷��̾� ���� * ���� ����
            float attackRate = player.stats.attackSpeed * player.stats.weapon.attackSpeed;

            // ����
            yield return new WaitForSeconds(preDelay / attackRate);

            // ����� ��ġ�� ����
            if (effect != null)
                Destroy(effect);
            effect = Instantiate(WheelWindEffect, user.transform.position, user.transform.rotation);
            effect.transform.parent = user.transform;


            // ���� ���� ����
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            effect.transform.localScale = new Vector3(size * player.stats.weapon.attackSize, size * player.stats.weapon.attackSize, 0);
            /*
            ����ü = false
            ����� = -1
            �ٴ���Ʈ = true
            �ʴ� Ÿ�� Ƚ�� = DPS * attackRate 
            �Ӽ� = ���� �Ӽ�
            ���ط� = (�⺻ ���ط� + ���� ���ط�) * �÷��̾� ���ݷ�
            �˹� = ���� �˹�
            ġȮ = �÷��̾� ġȮ
            ġ�� = �÷��̾� ġ��
            ����� = ����
            */
            hitDetection.SetHitDetection(false, -1, true, (int)(DPS * attackRate),
             player.stats.weapon.attackAttribute,
             (player.stats.weapon.damage + damage) * player.stats.power,
             player.stats.weapon.knockBack,
             player.stats.critical,
             player.stats.criticalDamage,
             player.stats.weapon.deBuff);
        }
    }

    public override void Exit(GameObject user)
    {
        StartCoroutine("AttackOut");

    }

    IEnumerator AttackOut()
    {
        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();

            float attackRate = player.stats.attackSpeed * player.stats.weapon.attackSpeed;

            yield return new WaitForSeconds(postDelay / attackRate);

            Destroy(effect);

        }
    }
}
