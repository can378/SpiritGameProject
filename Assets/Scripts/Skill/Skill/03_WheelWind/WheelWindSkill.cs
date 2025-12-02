using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWindSkill : SkillBase
{
    [field: SerializeField] WheelWindSkillData WWSData;

    //占쏙옙占쏙옙트
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
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[weapon.weaponInstance.weaponData.weaponType.ToString()];
            float attackRate = animationInfo.GetSPA() / player.playerStats.AttackSpeed.Value;

            skillCoolTime = 99;

            // 占쏙옙占쏙옙 占쌈듸옙 占쏙옙占쏙옙
            player.stats.MoveSpeed.DecreasedValue += 0.5f;
            
            // 占시곤옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙 회占쏙옙 占쏙옙占쏙옙
            yield return new WaitForSeconds(WWSData.preDelay * attackRate);

            // 占쏙옙占쏙옙占? 占쏙옙치占쏙옙 占쏙옙占쏙옙
            if (WheelWindEffect != null)
                Destroy(WheelWindEffect);

            WheelWindEffect = Instantiate(WWSData.WheelWindPrefab, user.transform.position, user.transform.rotation);
            WheelWindEffect.transform.parent = user.transform;
            WheelWindEffect.transform.localScale = new Vector3(WWSData.defaultSize * weapon.GetAttackSize(), WWSData.defaultSize * weapon.GetAttackSize(), 1);
            WheelWindEffect.tag = "PlayerAttack";
            WheelWindEffect.layer = LayerMask.NameToLayer("PlayerAttack");

            // 占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙
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

            // 占쏙옙占쏙옙 占쌈듸옙 占쏙옙占쏙옙
            enemy.stats.MoveSpeed.DecreasedValue += 0.5f;

            // 占쏙옙타占쏙옙 占쏙옙占쏙옙
            skillCoolTime = WWSData.skillDefalutCoolTime;

            // 占시곤옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙 회占쏙옙 占쏙옙占쏙옙
            yield return new WaitForSeconds(WWSData.preDelay);

            // 占쏙옙占쏙옙占? 占쏙옙치占쏙옙 占쏙옙占쏙옙
            if (WheelWindEffect != null)
                Destroy(WheelWindEffect);

            WheelWindEffect = Instantiate(WWSData.WheelWindPrefab, user.transform.position, user.transform.rotation);
            WheelWindEffect.transform.parent = user.transform;
            WheelWindEffect.transform.localScale = new Vector3(WWSData.defaultSize, WWSData.defaultSize, 0);
            WheelWindEffect.tag = "EnemyAttack";
            WheelWindEffect.layer = LayerMask.NameToLayer("EnemyAttack");

            // 占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙
            hitDetection = WheelWindEffect.GetComponent<HitDetection>();
            /*
            占쏙옙占쏙옙체 = false
            占쏙옙占쏙옙占? = -1
            占쌕댐옙占쏙옙트 = true
            占십댐옙 타占쏙옙 횟占쏙옙 = DPS * attackRate 
            占쌈쇽옙 = 占쏙옙占쏙옙 占쌈쇽옙
            占쏙옙占쌔뤄옙 = (占썩본 占쏙옙占쌔뤄옙 + 占쏙옙占쏙옙 占쏙옙占쌔뤄옙) * 占시뤄옙占싱억옙 占쏙옙占쌥뤄옙
            占싯뱄옙 = 占쏙옙占쏙옙 占싯뱄옙
            치확 = 占시뤄옙占싱억옙 치확
            치占쏙옙 = 占시뤄옙占싱억옙 치占쏙옙
            占쏙옙占쏙옙占? = 占쏙옙占쏙옙
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
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[weapon.weaponInstance.weaponData.weaponType.ToString()];
            float attackRate = animationInfo.GetSPA() / player.playerStats.AttackSpeed.Value;

            yield return new WaitForSeconds(0.5f * attackRate);

            Destroy(WheelWindEffect);

            // 占쏙옙占쏙옙 占시곤옙占쏙옙 占쏙옙占쏙옙 占쏙옙 占쌈듸옙 占쏙옙占쏙옙 占쏙옙占쏙옙
            yield return new WaitForSeconds(WWSData.postDelay * attackRate);

            // 占쏙옙타占쏙옙 占쏙옙占쏙옙
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * WWSData.skillDefalutCoolTime;

            player.stats.MoveSpeed.DecreasedValue -= 0.5f;
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();

            yield return new WaitForSeconds(0.5f);

            // 회占쏙옙 占쏙옙占쏙옙
            Destroy(WheelWindEffect);

            // 占쏙옙占쏙옙 占시곤옙占쏙옙 占쏙옙占쏙옙 占쏙옙 占쌈듸옙 占쏙옙占쏙옙 占쏙옙占쏙옙
            yield return new WaitForSeconds(WWSData.postDelay);

            // 占쏙옙타占쏙옙 占쏙옙占쏙옙
            skillCoolTime = 5;

            enemy.stats.MoveSpeed.DecreasedValue -= 0.5f;
        }
    }

}
