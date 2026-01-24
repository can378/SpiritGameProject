using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderboltSkill : SkillBase
{
    [field: SerializeField] ThunderboltSkillData TSData;

    //소환 범위 시뮬
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

            // 사용자 위치에 생성
            if (simul != null)
                Destroy(simul);

            simul = Instantiate(TSData.summonAreaSimul, player.CenterPivot.transform.position, user.transform.rotation);
            simul.transform.localScale = new Vector3(TSData.summonAreaSize, TSData.summonAreaSize, 0);

            // 선딜의 시간이 지난 후 시작
            yield return new WaitForSeconds(TSData.preDelay);

            while (player.playerStatus.isSkillHold)
            {
                // 무작위 생성 위치 선정
                Vector3 pos = player.CenterPivot.transform.position + (Random.insideUnitSphere * TSData.summonAreaSize /2);
                pos.z = 0;
                GameObject effect = ObjectPoolManager.instance.Get(TSData.thunderboltEffectSimul, pos);
                HitDetection hitDetection = effect.GetComponent<HitDetection>();

                effect.transform.localScale = new Vector3(TSData.defaultSize, TSData.defaultSize, 0);
                effect.tag = "PlayerAttack";
                effect.layer = LayerMask.NameToLayer("PlayerAttack");

                hitDetection.SetHit_Ratio(
                TSData.defaultDamage, TSData.ratio, player.playerStats.SkillPower,
                TSData.knockBack);
                //hitDetection.SetSEs(TSData.statusEffect);

                //Destroy(effect, TSData.effectTime);

                yield return new WaitForSeconds(1f/ TSData.DPS);
            }
        }
        else if (user.tag == "Enemy")
        {

            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer = 0;

            skillCoolTime = 99;

            // 사용자 위치에 생성
            if (simul != null)
                Destroy(simul);

            simul = Instantiate(TSData.summonAreaSimul, enemy.CenterPivot.transform.position, user.transform.rotation);
            simul.transform.localScale = new Vector3(TSData.summonAreaSize, TSData.summonAreaSize, 0);

            // 선딜의 시간이 지난 후 시작
            yield return new WaitForSeconds(TSData.preDelay);

            while (timer <= TSData.maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                // 무작위 생성 위치 선정
                Vector3 pos = enemy.CenterPivot.transform.position + (Random.insideUnitSphere * TSData.summonAreaSize / 2);
                pos.z = 0;
                GameObject effect = ObjectPoolManager.instance.Get(TSData.thunderboltEffectSimul, pos);
                HitDetection hitDetection = effect.GetComponent<HitDetection>();

                effect.transform.localScale = new Vector3(TSData.defaultSize, TSData.defaultSize, 0);
                effect.tag = "EnemyAttack";
                effect.layer = LayerMask.NameToLayer("EnemyAttack");

                hitDetection.SetHit_Ratio(
                TSData.defaultDamage, TSData.ratio, enemy.stats.SkillPower,
                TSData.knockBack);
                //hitDetection.SetSEs(TSData.statusEffect);
                hitDetection.user = user;

                //Destroy(effect, TSData.effectTime);

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


            // 조금 시간이 지난 후 속도 감소 해제
            yield return new WaitForSeconds(TSData.postDelay);

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * TSData.skillDefalutCoolTime;

        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();

            yield return new WaitForSeconds(TSData.effectTime);

            Destroy(simul);

            // 조금 시간이 지난 후 속도 감소 해제
            yield return new WaitForSeconds(TSData.postDelay);

            // 쿨타임 적용
            skillCoolTime = 5;

        }
    }

}
