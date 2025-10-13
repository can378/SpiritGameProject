using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSkill : SkillBase
{
    [field: SerializeField] WaveSkillData WSData;
    protected void Awake()
    {
        skillData = WSData;
    }
    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        Attack();
    }

    public override void Cancle()
    {
        
    }

    public override void Exit()
    {

    }

    void Attack()
    {
        Debug.Log("Wave");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            GameObject effect = Instantiate(WSData.waveEffectPrefab, player.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * WSData.skillDefalutCoolTime;

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
             WSData.defaultDamage, WSData.ratio, player.stats.AttackPower,
             10);
            hitDetection.SetSE(WSData.statusEffect);
            hitDetection.user = user;

            // Growing ���
            hitDetection.SetGrowing(true, 3);

            // rate ���� ����
            Destroy(effect, WSData.effectTime);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(WSData.waveEffectPrefab, enemy.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // ��Ÿ�� ����
            skillCoolTime = 5;


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
             WSData.defaultDamage, WSData.ratio, enemy.stats.SkillPower,
             10);
            hitDetection.SetSE(WSData.statusEffect);
            hitDetection.user = user;

            // Growing ���
            hitDetection.SetGrowing(true, 3);

            // rate ���� ����
            Destroy(effect, WSData.effectTime);
        }
    }

    
}
