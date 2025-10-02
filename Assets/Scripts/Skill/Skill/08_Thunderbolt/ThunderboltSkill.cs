using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderboltSkill : SkillBase
{
    [field: SerializeField] ThunderboltSkillData TSData;

    //��ȯ ���� �ù�
    GameObject simul;
    protected void Awake()
    {
        skillData = TSData;
    }
    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("Thunderbolt");

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();

            skillCoolTime = 99;

            // ���� �ӵ� ����
            player.stats.MoveSpeed.DecreasedValue += 99f;

            // ����� ��ġ�� ����
            if (simul != null)
                Destroy(simul);

            simul = Instantiate(TSData.summonAreaSimul, player.CenterPivot.transform.position, user.transform.rotation);
            simul.transform.localScale = new Vector3(TSData.summonAreaSize, TSData.summonAreaSize, 0);

            // ������ �ð��� ���� �� ����
            yield return new WaitForSeconds(TSData.preDelay);

            while (player.playerStatus.isSkillHold)
            {
                // ������ ���� ��ġ ����
                Vector3 pos = player.CenterPivot.transform.position + (Random.insideUnitSphere * TSData.summonAreaSize /2);
                pos.z = 0;
                GameObject effect = Instantiate(TSData.thunderboltEffectSimul, pos, Quaternion.identity);
                HitDetection hitDetection = effect.GetComponent<HitDetection>();

                effect.transform.localScale = new Vector3(TSData.defaultSize, TSData.defaultSize, 0);
                effect.tag = "PlayerAttack";
                effect.layer = LayerMask.NameToLayer("PlayerAttack");

                hitDetection.SetHit_Ratio(
                TSData.defaultDamage, TSData.ratio, player.playerStats.SkillPower,
                TSData.knockBack);
                hitDetection.SetSEs(TSData.statusEffect);

                Destroy(effect, TSData.effectTime);

                yield return new WaitForSeconds(1f/ TSData.DPS);
            }
        }
        else if (user.tag == "Enemy")
        {

            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer = 0;

            skillCoolTime = 99;

            // ���� �ӵ� ����
            enemy.stats.MoveSpeed.DecreasedValue += 99f;

            

            // ����� ��ġ�� ����
            if (simul != null)
                Destroy(simul);

            simul = Instantiate(TSData.summonAreaSimul, enemy.CenterPivot.transform.position, user.transform.rotation);
            simul.transform.localScale = new Vector3(TSData.summonAreaSize, TSData.summonAreaSize, 0);

            // ������ �ð��� ���� �� ����
            yield return new WaitForSeconds(TSData.preDelay);

            while (timer <= TSData.maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                // ������ ���� ��ġ ����
                Vector3 pos = enemy.CenterPivot.transform.position + (Random.insideUnitSphere * TSData.summonAreaSize / 2);
                pos.z = 0;
                GameObject effect = Instantiate(TSData.thunderboltEffectSimul, pos, Quaternion.identity);
                HitDetection hitDetection = effect.GetComponent<HitDetection>();

                effect.transform.localScale = new Vector3(TSData.defaultSize, TSData.defaultSize, 0);
                effect.tag = "EnemyAttack";
                effect.layer = LayerMask.NameToLayer("EnemyAttack");

                hitDetection.SetHit_Ratio(
                TSData.defaultDamage, TSData.ratio, enemy.stats.SkillPower,
                TSData.knockBack);
                hitDetection.SetSEs(TSData.statusEffect);
                hitDetection.user = user;

                Destroy(effect, TSData.effectTime);

                timer += 1f/ TSData.DPS;

                yield return new WaitForSeconds(1f / TSData.DPS);
            }
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        StartCoroutine(AttackOut());
        Destroy(simul);
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

            yield return new WaitForSeconds(TSData.effectTime);

            Destroy(simul);

            // ���� �ð��� ���� �� �ӵ� ���� ����
            yield return new WaitForSeconds(TSData.postDelay);

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * TSData.skillDefalutCoolTime;

            player.stats.MoveSpeed.DecreasedValue -= 99f;
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();

            yield return new WaitForSeconds(TSData.effectTime);

            Destroy(simul);

            // ���� �ð��� ���� �� �ӵ� ���� ����
            yield return new WaitForSeconds(TSData.postDelay);

            // ��Ÿ�� ����
            skillCoolTime = 5;

            enemy.stats.MoveSpeed.DecreasedValue -= 99f;
        }
    }

}
