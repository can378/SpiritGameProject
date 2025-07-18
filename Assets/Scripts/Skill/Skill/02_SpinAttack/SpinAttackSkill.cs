using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackSkill : Skill
{
    // 피해량
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;

    // 최대 강화량
    [field: SerializeField] float maxHoldPower;

    // 기본 크기, 이펙트 유지시간, 이펙트
    [field: SerializeField] float size;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject spinPrefab;
    [field: SerializeField] GameObject spinSimulPrefab;

    // 강화 수치
    float holdPower;
    GameObject spinSimul;

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

            spinSimul = Instantiate(spinSimulPrefab, player.CenterPivot.transform.position, Quaternion.identity);
            spinSimul.transform.parent = user.transform;

            while (holdPower < maxHoldPower && player.playerStatus.isSkillHold)
            {
                holdPower += 0.05f;
                spinSimul.transform.localScale = new Vector3(holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer;

            enemy.stats.MoveSpeed.DecreasedValue += 0.5f;

            spinSimul = Instantiate(spinSimulPrefab, enemy.CenterPivot.transform.position, Quaternion.identity);
            spinSimul.transform.parent = user.transform;

            holdPower = 1f;
            timer = 0;

            while (holdPower < maxHoldPower && timer <= maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                holdPower += 0.05f;
                spinSimul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
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
            Weapon weapon = player.weaponList[player.playerStats.weapon];
            GameObject effect = Instantiate(spinPrefab, player.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[weapon.weaponType.ToString()];

            float attackRate = animationInfo.GetSPA() / player.playerStats.attackSpeed;

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, 0);
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
             defalutDamage * holdPower, ratio * holdPower, player.stats.AttackPower,
             player.weaponList[player.playerStats.weapon].knockBack * holdPower, 
             player.playerStats.CriticalChance, 
             player.playerStats.CriticalDamage);
            hitDetection.SetSE((int)player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;

            // 파티클
            {
                // 공격 속도에 따른 이펙트 가속
                ParticleSystem.MainModule particleMain = effect.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startLifetime = animationInfo.Rate / player.playerStats.attackSpeed;
            }

            // rate 동안 유지
            player.stats.MoveSpeed.DecreasedValue -= 0.5f;
            Destroy(effect, time * attackRate);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(spinPrefab, enemy.CenterPivot.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
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
             defalutDamage * holdPower, ratio * holdPower, enemy.stats.AttackPower,
             10 * holdPower);
            hitDetection.user = user;

            // rate 동안 유지
            enemy.stats.MoveSpeed.DecreasedValue -= 0.5f;
            Destroy(effect, time);
        }
    }

    
}
