using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderboltSkill : Skill
{
    //���ط�
    [field: SerializeField] int defaultDamage;
    [field: SerializeField] float ratio;


    // �ʴ� ���� Ƚ��, ���� ũ��, �˹�, ���� ���� �ð�, ��ȯ ����, ��ȯ ���� �ù�, ����, �����̻�
    [field: SerializeField] float DPS;
    [field: SerializeField] float size;
    [field: SerializeField] float knockBack;
    [field: SerializeField] float time;
    [field: SerializeField] float summonAreaSize;
    [field: SerializeField] GameObject summonAreaSimul;
    [field: SerializeField] GameObject thunderbolt;
    [field: SerializeField] int[] statusEffect;

    //��ȯ ���� �ù�
    GameObject simul;

    public override void Enter(GameObject user)
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
            player.stats.decreasedMoveSpeed += 99f;

            // ����� ��ġ�� ����
            if (simul != null)
                Destroy(simul);

            simul = Instantiate(summonAreaSimul, user.transform.position, user.transform.rotation);
            simul.transform.localScale = new Vector3(summonAreaSize, summonAreaSize, 0);

            // ������ �ð��� ���� �� ����
            yield return new WaitForSeconds(preDelay);

            while (player.playerStatus.isSkillHold)
            {
                // ������ ���� ��ġ ����
                Vector3 pos = transform.position + (Random.insideUnitSphere * summonAreaSize/2);
                pos.z = 0;
                GameObject effect = Instantiate(thunderbolt, pos, Quaternion.identity);
                HitDetection hitDetection = effect.GetComponent<HitDetection>();

                effect.transform.localScale = new Vector3(size, size, 0);
                effect.tag = "PlayerAttack";
                effect.layer = LayerMask.NameToLayer("PlayerAttack");

                hitDetection.SetHitDetection(false, -1, false, -1,
                defaultDamage + player.playerStats.skillPower * ratio,
                knockBack);
                hitDetection.SetSEs(statusEffect);

                Destroy(effect, time);

                yield return new WaitForSeconds(1f/ DPS);
            }
        }
        else if (user.tag == "Enemy")
        {

            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer = 0;

            skillCoolTime = 99;

            // ���� �ӵ� ����
            enemy.stats.decreasedMoveSpeed += 99f;

            

            // ����� ��ġ�� ����
            if (simul != null)
                Destroy(simul);

            simul = Instantiate(summonAreaSimul, user.transform.position, user.transform.rotation);
            simul.transform.localScale = new Vector3(summonAreaSize, summonAreaSize, 0);

            // ������ �ð��� ���� �� ����
            yield return new WaitForSeconds(preDelay);

            while (timer <= maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                // ������ ���� ��ġ ����
                Vector3 pos = transform.position + (Random.insideUnitSphere * summonAreaSize / 2);
                pos.z = 0;
                GameObject effect = Instantiate(thunderbolt, pos, Quaternion.identity);
                HitDetection hitDetection = effect.GetComponent<HitDetection>();

                effect.transform.localScale = new Vector3(size, size, 0);
                effect.tag = "EnemyAttack";
                effect.layer = LayerMask.NameToLayer("EnemyAttack");

                hitDetection.SetHitDetection(false, -1, false, -1,
                defaultDamage,
                knockBack);
                hitDetection.SetSEs(statusEffect);
                hitDetection.user = user;

                Destroy(effect, time);

                timer += 1f/ DPS;

                yield return new WaitForSeconds(1f / DPS);
            }
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        StartCoroutine(AttackOut());
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

            yield return new WaitForSeconds(time);

            Destroy(simul);

            // ���� �ð��� ���� �� �ӵ� ���� ����
            yield return new WaitForSeconds(postDelay);

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            player.stats.decreasedMoveSpeed -= 99f;
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();

            yield return new WaitForSeconds(time);

            Destroy(simul);

            // ���� �ð��� ���� �� �ӵ� ���� ����
            yield return new WaitForSeconds(postDelay);

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;

            enemy.stats.decreasedMoveSpeed -= 99f;
        }
    }

}
