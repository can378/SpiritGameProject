using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackSkill : SkillBase
{
    [field: SerializeField] SpinAttackSkillData SASData;

    
    float holdPower;        // 강화 수치
    GameObject spinSimul;   // 작동 중인 시뮬레이션

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
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[weapon.weaponInstance.weaponData.weaponType.ToString()];

            float attackRate = animationInfo.GetSPA() / player.playerStats.AttackSpeed.Value;

            holdPower = SASData.maxHoldPower < holdPower ? SASData.maxHoldPower : holdPower; 

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * SASData.skillDefalutCoolTime;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * SASData.defaultSize * weapon.GetAttackSize(), holdPower * SASData.defaultSize * weapon.GetAttackSize(), 0);
            effect.tag = "PlayerAttack";
            effect.layer = LayerMask.NameToLayer("PlayerAttack");
            /*
            투사체 = false
            관통력 = -1
            다단히트 = false
            초당 타격 횟수 = -1 
            피해량 = (기본 피해량 + 무기 피해량) * 플레이어 공격력
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 없음
            */
            hitDetection.SetHit_Ratio(
             SASData.defaultDamage * holdPower, SASData.ratio * holdPower, player.stats.AttackPower,
             weapon.GetKnockBack() * holdPower, 
             player.playerStats.CriticalChance, 
             player.playerStats.CriticalDamage);
            //hitDetection.SetSE((int)player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;

            // 파티클
            {
                // 공격 속도에 따른 이펙트 가속
                ParticleSystem.MainModule particleMain = effect.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startLifetime = animationInfo.Rate / player.playerStats.AttackSpeed.Value;
            }

            // rate 동안 유지
            player.stats.MoveSpeed.DecreasedValue -= 0.5f;
            Destroy(effect, SASData.effectTime * attackRate);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(SASData.spinPrefab, enemy.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 쿨타임 적용
            skillCoolTime = 5;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * SASData.defaultSize, holdPower * SASData.defaultSize, 0);
            effect.tag = "EnemyAttack";
            effect.layer = LayerMask.NameToLayer("EnemyAttack");

            /*
            투사체 = false
            관통력 = -1
            다단히트 = false
            초당 타격 횟수 = -1 
            피해량 = (기본 피해량 + 무기 피해량) * 플레이어 공격력
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 없음
            */
            hitDetection.SetHit_Ratio(
             SASData.defaultDamage * holdPower, SASData.ratio * holdPower, enemy.stats.AttackPower,
             10 * holdPower);
            hitDetection.user = user;

            // rate 동안 유지
            enemy.stats.MoveSpeed.DecreasedValue -= 0.5f;
            Destroy(effect, SASData.effectTime);
        }
    }

    
}
