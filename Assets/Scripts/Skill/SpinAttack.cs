using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : Skill
{
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public GameObject spinEffect { get; private set; }

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("SpinAttack");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.stats.decreasedSkillCoolTime) * skillDefalutCoolTime;

            // ���� = �÷��̾� ���� * ���� ����
            float attackRate = player.stats.attackSpeed * player.stats.weapon.attackSpeed;

            // ����
            yield return new WaitForSeconds(preDelay / attackRate);

            // ����� ��ġ�� ����
            GameObject instant = Instantiate(spinEffect, user.transform.position, user.transform.rotation);

            // ���� ���� ����
            HitDetection hitDetection = instant.GetComponent<HitDetection>();

            // ũ�� ����
            instant.transform.localScale = new Vector3(size * player.stats.weapon.attackSize, size * player.stats.weapon.attackSize, 0);

            /*
            ����ü = false
            ����� = -1
            �ٴ���Ʈ = false
            �ʴ� Ÿ�� Ƚ�� = -1 
            �Ӽ� = ���� �Ӽ�
            ���ط� = (�⺻ ���ط� + ���� ���ط�) * �÷��̾� ���ݷ�
            �˹� = ���� �˹�
            ġȮ = �÷��̾� ġȮ
            ġ�� = �÷��̾� ġ��
            ����� = ����
            */
            hitDetection.SetHitDetection(false, -1, false, -1,
             player.stats.weapon.attackAttribute, 
             (player.stats.weapon.damage + damage) * player.stats.power,
             player.stats.weapon.knockBack, 
             player.stats.critical, 
             player.stats.criticalDamage,
             player.stats.weapon.deBuff);

            // rate ���� ����
            Destroy(instant, rate / attackRate);

            // �ĵ�
            // ���ֵ� �� ��
            yield return new WaitForSeconds(postDelay / attackRate);
        }
    }

    public override void Exit(GameObject user)
    {
        
    }
}
