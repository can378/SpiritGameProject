using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : Skill
{
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }
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
            skillCoolTime = player.stats.skillCoolTime * skillDefalutCoolTime;

            // ���� = �÷��̾� ���� * ���� ����
            float attackSpeed = player.stats.attackSpeed * player.stats.weapon.attackSpeed;

            // ����
            yield return new WaitForSeconds(preDelay / attackSpeed);

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
             defalutDamage + player.stats.attackPower * ratio,
             player.stats.weapon.knockBack, 
             player.stats.criticalChance, 
             player.stats.criticalDamage,
             player.stats.weapon.deBuff);

            // rate ���� ����
            Destroy(instant, rate / attackSpeed);

            // �ĵ�
            // ���ֵ� �� ��
            yield return new WaitForSeconds(postDelay / attackSpeed);
        }
    }

    public override void Exit(GameObject user)
    {
        
    }
}
