using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : Skill
{
    // ���ط�, �з���
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;
    [field: SerializeField] float knockBack;

    // ��Ÿ�, ũ��, �����ð�, v������, �ùķ����� ������, �����̻�
    [field: SerializeField] float range;
    [field: SerializeField] float size;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject fireBallPrefab;
    [field: SerializeField] GameObject simulPrefab;
    [field: SerializeField] int[] statusEffect;

    //�ߵ� �� ȿ�� ���� ǥ�ñ�
    Transform fireBallSimul;
    Transform rangeSimul;


    public override void Enter(GameObject user)
    {
        base.Enter(user);
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if(user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            fireBallSimul = Instantiate(simulPrefab, player.status.mousePos, Quaternion.identity).transform;
            fireBallSimul.transform.localScale = new Vector3(size, size, 1);

            rangeSimul = Instantiate(simulPrefab,player.transform.position,Quaternion.identity).transform;
            rangeSimul.parent = player.transform;
            rangeSimul.localScale = new Vector3(range * 2 , range * 2, 1);
            
            while (player.status.isSkillHold)
            {
                fireBallSimul.position = player.transform.position + Vector3.ClampMagnitude(player.status.mousePos - player.transform.position, range);
                yield return null;
            }
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float timer = 0;

            fireBallSimul = Instantiate(simulPrefab, enemy.enemyTarget.transform.position, Quaternion.identity).transform;
            fireBallSimul.localScale = new Vector3(size, size, 1);

            rangeSimul = Instantiate(simulPrefab, enemy.transform.position, Quaternion.identity).transform;
            rangeSimul.parent = enemy.transform;
            rangeSimul.localScale = new Vector3(range * 2, range * 2, 1);

            while (timer <= maxHoldTime/2)
            {
                // ���߿� �� ���·� �ִ� ���� �����ϱ�
                // ���߿� �� ���·� �ִ� ���� ǥ���ϱ�
                fireBallSimul.position = enemy.transform.position + Vector3.ClampMagnitude(enemy.enemyTarget.transform.position - enemy.transform.position, range);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void Exit()
    {
        StopCoroutine(Simulation());
        Fire();
    }

    void Fire()
    {
        Debug.Log("FireBall");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();
            GameObject effect = Instantiate(fireBallPrefab, fireBallSimul.position, Quaternion.identity);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            Destroy(rangeSimul.gameObject);
            Destroy(fireBallSimul.gameObject);
            Destroy(effect, time);

            effect.transform.localScale = new Vector3(size, size, 1);
            effect.tag = "PlayerAttack";
            effect.layer = LayerMask.NameToLayer("PlayerAttack");

            /*
            ����ü = false
            ����� = -1
            �ٴ���Ʈ = false
            �ʴ� Ÿ�� Ƚ�� = -1 
            ���ط� = ���ط� * �÷��̾� �ֹ���
            �˹� = �˹�
            ġȮ = 0
            ġ�� = 0
            ����� = ȭ��
            */
            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + player.playerStats.skillPower * ratio, knockBack, 0, 0, statusEffect);
            hitDetection.user = user;
            
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(fireBallPrefab, fireBallSimul.transform.position, Quaternion.identity);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            skillCoolTime = skillDefalutCoolTime;

            // ����Ʈ ����
            Destroy(rangeSimul.gameObject);
            Destroy(fireBallSimul.gameObject);
            Destroy(effect, time);

            effect.transform.localScale = new Vector3(size, size, 1);
            effect.tag = "EnemyAttack";
            effect.layer = LayerMask.NameToLayer("EnemyAttack");

            /*
            ����ü = false
            ����� = -1
            �ٴ���Ʈ = false
            �ʴ� Ÿ�� Ƚ�� = -1 
            ���ط� = ���ط� * �÷��̾� �ֹ���
            �˹� = �˹�
            ġȮ = 0
            ġ�� = 0
            ����� = ȭ��
            */
            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + enemy.stats.attackPower * ratio, knockBack, 0, 0, statusEffect);
            hitDetection.user = user;
        }
    }
}
