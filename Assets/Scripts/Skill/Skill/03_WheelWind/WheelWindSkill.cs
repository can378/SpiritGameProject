using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWindSkill : Skill
{
    //���ط�
    [field: SerializeField] int defaultDamage;
    [field: SerializeField] float ratio;


    // �ʴ� Ÿ�� Ƚ��, ũ��, ����Ʈ
    [field: SerializeField] int DPS;
    [field: SerializeField] float size;
    [field: SerializeField] GameObject WheelWindPrefab;

    //����Ʈ
    GameObject WheelWindEffect;

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("WheelWind");

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponList[player.playerStats.weapon];
            HitDetection hitDetection;
            float attackRate = weapon.SPA / player.playerStats.attackSpeed;

            skillCoolTime = 99;

            // ���� �ӵ� ����
            player.stats.decreasedMoveSpeed += 0.5f;
            
            // �ð��� ���� ���� �� ȸ�� ����
            yield return new WaitForSeconds(preDelay * attackRate);

            // ����� ��ġ�� ����
            if (WheelWindEffect != null)
                Destroy(WheelWindEffect);

            WheelWindEffect = Instantiate(WheelWindPrefab, user.transform.position, user.transform.rotation);
            WheelWindEffect.transform.parent = user.transform;
            WheelWindEffect.transform.localScale = new Vector3(size * player.weaponList[player.playerStats.weapon].attackSize, size * player.weaponList[player.playerStats.weapon].attackSize, 1);
            WheelWindEffect.tag = "PlayerAttack";
            WheelWindEffect.layer = LayerMask.NameToLayer("PlayerAttack");

            // ���� ���� ����
            hitDetection = WheelWindEffect.GetComponent<HitDetection>();
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
            hitDetection.SetHitDetection(false, -1, true, (int)((float)DPS / attackRate),
             defaultDamage + player.stats.attackPower * ratio,
             player.weaponList[player.playerStats.weapon].knockBack,
             player.playerStats.criticalChance,
             player.playerStats.criticalDamage,
             player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            HitDetection hitDetection;

            skillCoolTime = 99;

            // ���� �ӵ� ����
            enemy.stats.decreasedMoveSpeed += 0.5f;

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;

            // �ð��� ���� ���� �� ȸ�� ����
            yield return new WaitForSeconds(preDelay);

            // ����� ��ġ�� ����
            if (WheelWindEffect != null)
                Destroy(WheelWindEffect);

            WheelWindEffect = Instantiate(WheelWindPrefab, user.transform.position, user.transform.rotation);
            WheelWindEffect.transform.parent = user.transform;
            WheelWindEffect.transform.localScale = new Vector3(size, size, 0);
            WheelWindEffect.tag = "EnemyAttack";
            WheelWindEffect.layer = LayerMask.NameToLayer("EnemyAttack");

            // ���� ���� ����
            hitDetection = WheelWindEffect.GetComponent<HitDetection>();
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
            hitDetection.SetHitDetection(false, -1, true, DPS,
             defaultDamage + enemy.stats.attackPower * ratio,
             1,
             0,
             0,
             null);
            hitDetection.user = user;
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        Destroy(WheelWindEffect);
        user.GetComponent<Stats>().decreasedMoveSpeed -= 0.5f;
    }


    public override void Exit()
    {
        StartCoroutine(AttackOut());
    }

    IEnumerator AttackOut()
    {
        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponList[player.playerStats.weapon];
            float attackRate = weapon.SPA / player.playerStats.attackSpeed;

            yield return new WaitForSeconds(0.5f * attackRate);

            Destroy(WheelWindEffect);

            // ���� �ð��� ���� �� �ӵ� ���� ����
            yield return new WaitForSeconds(postDelay * attackRate);

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            player.stats.decreasedMoveSpeed -= 0.5f;
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();

            yield return new WaitForSeconds(0.5f);

            // ȸ�� ����
            Destroy(WheelWindEffect);

            // ���� �ð��� ���� �� �ӵ� ���� ����
            yield return new WaitForSeconds(postDelay);

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;

            enemy.stats.decreasedMoveSpeed -= 0.5f;
        }
    }

}
