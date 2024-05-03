using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : Skill
{
    // ���ط�
    [field: SerializeField] public int defalutDamage {get; private set;}
    [field: SerializeField] public float ratio { get; private set; }

    // ũ��, �˹�, ����Ʈ �����ð�, ����Ʈ, �����̻�
    [field: SerializeField] float size;
    [field: SerializeField] float knockBack;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject fireBallEffect;
    [field: SerializeField] GameObject fireBallEffectSimul;
    [field: SerializeField] int[] statusEffect;

    //�ߵ� �� ȿ�� ���� ǥ�ñ�
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
            Player player = user.GetComponent<Player>();

            simul = Instantiate(fireBallEffectSimul, player.status.mousePos, Quaternion.identity);
            simul.transform.localScale = new Vector3(size, size, 0);
            
            while (player.status.isSkillHold)
            {
                // ���߿� �� ���·� �ִ� ���� �����ϱ�
                // ���߿� �� ���·� �ִ� ���� ǥ���ϱ�
                simul.transform.position = player.status.mousePos;
                yield return null;
            }
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float timer = 0;

            simul = Instantiate(fireBallEffectSimul, enemy.enemyTarget.transform.position, Quaternion.identity);
            simul.transform.localScale = new Vector3(size, size, 0);

            while (timer <= maxHoldTime/2)
            {
                // ���߿� �� ���·� �ִ� ���� �����ϱ�
                // ���߿� �� ���·� �ִ� ���� ǥ���ϱ�
                simul.transform.position = enemy.enemyTarget.transform.position;
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
            GameObject effect = Instantiate(fireBallEffect, simul.transform.position, Quaternion.identity);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            Destroy(simul);
            Destroy(effect, time);

            effect.transform.localScale = new Vector3(size, size, 0);
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
            hitDetection.SetHitDetection(false, -1, false, -1, defalutDamage + player.stats.skillPower * ratio, knockBack, 0, 0, statusEffect);

            
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(fireBallEffect, simul.transform.position, Quaternion.identity);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            skillCoolTime = skillDefalutCoolTime;

            // ����Ʈ ����
            Destroy(simul);
            Destroy(effect, time);

            effect.transform.localScale = new Vector3(size, size, 0);
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

           
        }
    }
}
