using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSkill : Skill
{
    // ���ط�
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }

    // �⺻ ũ��, ����Ʈ �����ð�, ����Ʈ
    [field: SerializeField] float time;
    [field: SerializeField] GameObject waveEffect;
    [field: SerializeField] int[] statusEffect;

    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        StartCoroutine(Attack());
    }

    public override void Cancle()
    {
        
    }

    public override void Exit()
    {

    }

    IEnumerator Attack()
    {
        Debug.Log("Wave");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            GameObject effect = Instantiate(waveEffect, player.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            effect.transform.localScale = new Vector3(1, 1, 1);
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
            hitDetection.SetHit_Ratio(
             defalutDamage , ratio, player.stats.AttackPower,
             10);
            hitDetection.SetSEs(statusEffect);
            hitDetection.user = user;

            // rate ���� ����
            Destroy(effect,time);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(waveEffect, enemy.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
            float timer = 0;

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;


            effect.transform.localScale = new Vector3(1, 1, 1);
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
            hitDetection.SetHit_Ratio(
             defalutDamage, ratio, enemy.stats.SkillPower,
             10);
            hitDetection.SetSEs(statusEffect);
            hitDetection.user = user;

            while (timer < time)
            {
                effect.transform.localScale = new Vector3(1 + timer * 10, 1 + timer * 10, 1);
                timer += Time.deltaTime;
                yield return null;
            }

            // rate ���� ����
            Destroy(effect,time);
        }
    }

    
}
