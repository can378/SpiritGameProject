using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWindSkill : SkillBase
{
    [field: SerializeField] WheelWindSkillData WWSData;

    //����Ʈ
    GameObject WheelWindEffect;

    protected void Awake()
    {
        skillData = WWSData;
    }

    public override void Enter(ObjectBasic user)
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
            PlayerWeapon weapon = player.playerStats.weapon;
            HitDetection hitDetection;
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[weapon.weaponData.weaponType.ToString()];
            float attackRate = animationInfo.GetSPA() / player.playerStats.attackSpeed;

            skillCoolTime = 99;

            // ���� �ӵ� ����
            player.stats.MoveSpeed.DecreasedValue += 0.5f;
            
            // �ð��� ���� ���� �� ȸ�� ����
            yield return new WaitForSeconds(WWSData.preDelay * attackRate);

            // �����? ��ġ�� ����
            if (WheelWindEffect != null)
                Destroy(WheelWindEffect);

            WheelWindEffect = Instantiate(WWSData.WheelWindPrefab, user.transform.position, user.transform.rotation);
            WheelWindEffect.transform.parent = user.transform;
            WheelWindEffect.transform.localScale = new Vector3(WWSData.defaultSize * weapon.GetAttackSize(), WWSData.defaultSize * weapon.GetAttackSize(), 1);
            WheelWindEffect.tag = "PlayerAttack";
            WheelWindEffect.layer = LayerMask.NameToLayer("PlayerAttack");

            // ���� ���� ����
            hitDetection = WheelWindEffect.GetComponent<HitDetection>();

            hitDetection.SetHit_Ratio(WWSData.defaultDamage, WWSData.ratio, player.stats.AttackPower,
             weapon.GetKnockBack(),
             player.playerStats.CriticalChance,
             player.playerStats.CriticalDamage);
            hitDetection.SetMultiHit(true, 1 / WWSData.DPS);
            //hitDetection.SetSE((int)player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            HitDetection hitDetection;

            skillCoolTime = 99;

            // ���� �ӵ� ����
            enemy.stats.MoveSpeed.DecreasedValue += 0.5f;

            // ��Ÿ�� ����
            skillCoolTime = WWSData.skillDefalutCoolTime;

            // �ð��� ���� ���� �� ȸ�� ����
            yield return new WaitForSeconds(WWSData.preDelay);

            // �����? ��ġ�� ����
            if (WheelWindEffect != null)
                Destroy(WheelWindEffect);

            WheelWindEffect = Instantiate(WWSData.WheelWindPrefab, user.transform.position, user.transform.rotation);
            WheelWindEffect.transform.parent = user.transform;
            WheelWindEffect.transform.localScale = new Vector3(WWSData.defaultSize, WWSData.defaultSize, 0);
            WheelWindEffect.tag = "EnemyAttack";
            WheelWindEffect.layer = LayerMask.NameToLayer("EnemyAttack");

            // ���� ���� ����
            hitDetection = WheelWindEffect.GetComponent<HitDetection>();
            /*
            ����ü = false
            �����? = -1
            �ٴ���Ʈ = true
            �ʴ� Ÿ�� Ƚ�� = DPS * attackRate 
            �Ӽ� = ���� �Ӽ�
            ���ط� = (�⺻ ���ط� + ���� ���ط�) * �÷��̾� ���ݷ�
            �˹� = ���� �˹�
            ġȮ = �÷��̾� ġȮ
            ġ�� = �÷��̾� ġ��
            �����? = ����
            */
            hitDetection.SetHit_Ratio(
             WWSData.defaultDamage, WWSData.ratio, enemy.stats.AttackPower,
             1);
            hitDetection.SetMultiHit(true,1/ WWSData.DPS);
            hitDetection.user = user;
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        Destroy(WheelWindEffect);
        user.GetComponent<Stats>().MoveSpeed.DecreasedValue -= 0.5f;
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
            PlayerWeapon weapon = player.playerStats.weapon;
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[weapon.weaponData.weaponType.ToString()];
            float attackRate = animationInfo.GetSPA() / player.playerStats.attackSpeed;

            yield return new WaitForSeconds(0.5f * attackRate);

            Destroy(WheelWindEffect);

            // ���� �ð��� ���� �� �ӵ� ���� ����
            yield return new WaitForSeconds(WWSData.postDelay * attackRate);

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * WWSData.skillDefalutCoolTime;

            player.stats.MoveSpeed.DecreasedValue -= 0.5f;
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();

            yield return new WaitForSeconds(0.5f);

            // ȸ�� ����
            Destroy(WheelWindEffect);

            // ���� �ð��� ���� �� �ӵ� ���� ����
            yield return new WaitForSeconds(WWSData.postDelay);

            // ��Ÿ�� ����
            skillCoolTime = 5;

            enemy.stats.MoveSpeed.DecreasedValue -= 0.5f;
        }
    }

}
