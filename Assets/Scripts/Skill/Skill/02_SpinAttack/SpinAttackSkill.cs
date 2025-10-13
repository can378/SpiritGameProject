using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackSkill : SkillBase
{
    [field: SerializeField] SpinAttackSkillData SASData;

    
    float holdPower;        // ��ȭ ��ġ
    GameObject spinSimul;   // �۵� ���� �ùķ��̼�

    protected void Awake()
    {
        skillData = SASData;
    }

    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            player.stats.MoveSpeed.DecreasedValue += 0.5f;
            holdPower = 1f;

            spinSimul = Instantiate(SASData.spinSimulPrefab, player.CenterPivot.transform.position, Quaternion.identity);
            spinSimul.transform.parent = user.transform;

            while (holdPower < SASData.maxHoldPower && player.playerStatus.isSkillHold)
            {
                holdPower += 0.05f;
                spinSimul.transform.localScale = new Vector3(holdPower * SASData.defaultSize * player.playerStats.weapon.GetAttackSize(), holdPower * SASData.defaultSize * player.playerStats.weapon.GetAttackSize(), 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer;

            enemy.stats.MoveSpeed.DecreasedValue += 0.5f;

            spinSimul = Instantiate(SASData.spinSimulPrefab, enemy.CenterPivot.transform.position, Quaternion.identity);
            spinSimul.transform.parent = user.transform;

            holdPower = 1f;
            timer = 0;

            while (holdPower < SASData.maxHoldPower && timer <= SASData.maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                holdPower += 0.05f;
                spinSimul.transform.localScale = new Vector3(holdPower * SASData.defaultSize, holdPower * SASData.defaultSize, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        StopCoroutine(Simulation());
        user.GetComponent<Stats>().MoveSpeed.DecreasedValue -= 0.5f;
        Destroy(spinSimul);
    }

    public override void Exit()
    {
        StopCoroutine(Simulation());
        Attack();
    }

    void Attack()
    {
        Debug.Log("SpinAttack");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            PlayerWeapon weapon = player.playerStats.weapon;
            GameObject effect = Instantiate(SASData.spinPrefab, player.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[weapon.weaponData.weaponType.ToString()];

            float attackRate = animationInfo.GetSPA() / player.playerStats.attackSpeed;

            holdPower = SASData.maxHoldPower < holdPower ? SASData.maxHoldPower : holdPower; 

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * SASData.skillDefalutCoolTime;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * SASData.defaultSize * weapon.GetAttackSize(), holdPower * SASData.defaultSize * weapon.GetAttackSize(), 0);
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
             SASData.defaultDamage * holdPower, SASData.ratio * holdPower, player.stats.AttackPower,
             weapon.GetKnockBack() * holdPower, 
             player.playerStats.CriticalChance, 
             player.playerStats.CriticalDamage);
            //hitDetection.SetSE((int)player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;

            // ��ƼŬ
            {
                // ���� �ӵ��� ���� ����Ʈ ����
                ParticleSystem.MainModule particleMain = effect.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startLifetime = animationInfo.Rate / player.playerStats.attackSpeed;
            }

            // rate ���� ����
            player.stats.MoveSpeed.DecreasedValue -= 0.5f;
            Destroy(effect, SASData.effectTime * attackRate);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(SASData.spinPrefab, enemy.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // ��Ÿ�� ����
            skillCoolTime = 5;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * SASData.defaultSize, holdPower * SASData.defaultSize, 0);
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
             SASData.defaultDamage * holdPower, SASData.ratio * holdPower, enemy.stats.AttackPower,
             10 * holdPower);
            hitDetection.user = user;

            // rate ���� ����
            enemy.stats.MoveSpeed.DecreasedValue -= 0.5f;
            Destroy(effect, SASData.effectTime);
        }
    }

    
}
