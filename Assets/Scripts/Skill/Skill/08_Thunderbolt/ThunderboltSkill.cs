using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderboltSkill : Skill
{
    //피해량
    [field: SerializeField] int defaultDamage;
    [field: SerializeField] float ratio;


    // 초당 생성 횟수, 벼락 크기, 넉백, 벼락 유지 시간, 소환 범위, 소환 범위 시뮬, 벼락, 상태이상
    [field: SerializeField] float DPS;
    [field: SerializeField] float size;
    [field: SerializeField] float knockBack;
    [field: SerializeField] float time;
    [field: SerializeField] float summonAreaSize;
    [field: SerializeField] GameObject summonAreaSimul;
    [field: SerializeField] GameObject thunderbolt;
    [field: SerializeField] int[] statusEffect;

    //소환 범위 시뮬
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

            // 먼저 속도 감소
            player.stats.decreasedMoveSpeed += 99f;

            // 사용자 위치에 생성
            if (simul != null)
                Destroy(simul);

            simul = Instantiate(summonAreaSimul, user.transform.position, user.transform.rotation);
            simul.transform.localScale = new Vector3(summonAreaSize, summonAreaSize, 0);

            // 선딜의 시간이 지난 후 시작
            yield return new WaitForSeconds(preDelay);

            while (player.playerStatus.isSkillHold)
            {
                // 무작위 생성 위치 선정
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

            // 먼저 속도 감소
            enemy.stats.decreasedMoveSpeed += 99f;

            

            // 사용자 위치에 생성
            if (simul != null)
                Destroy(simul);

            simul = Instantiate(summonAreaSimul, user.transform.position, user.transform.rotation);
            simul.transform.localScale = new Vector3(summonAreaSize, summonAreaSize, 0);

            // 선딜의 시간이 지난 후 시작
            yield return new WaitForSeconds(preDelay);

            while (timer <= maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                // 무작위 생성 위치 선정
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

            // 조금 시간이 지난 후 속도 감소 해제
            yield return new WaitForSeconds(postDelay);

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            player.stats.decreasedMoveSpeed -= 99f;
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();

            yield return new WaitForSeconds(time);

            Destroy(simul);

            // 조금 시간이 지난 후 속도 감소 해제
            yield return new WaitForSeconds(postDelay);

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;

            enemy.stats.decreasedMoveSpeed -= 99f;
        }
    }

}
