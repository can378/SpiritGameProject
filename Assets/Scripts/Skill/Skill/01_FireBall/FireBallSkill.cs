using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSkill : SkillBase
{
    [field: SerializeField] FireBallSkillData FBSData;
    [field: SerializeField] public GameObject rangeSimul { get; private set; }

    [field: SerializeField] public GameObject fireBallSimul { get; private set; }

    protected void Awake()
    {
        skillData = FBSData;
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
            Player player = user.GetComponent<Player>();

            fireBallSimul.SetActive(true);
            fireBallSimul.transform.localScale = Vector3.one * 10;

            rangeSimul.SetActive(true);
            rangeSimul.transform.localScale = Vector3.one * FBSData.range * 2;
            
            while (player.playerStatus.isSkillHold)
            {
                fireBallSimul.transform.position = player.CenterPivot.transform.position + Vector3.ClampMagnitude(player.playerStatus.mousePos - player.CenterPivot.transform.position, FBSData.range);
                yield return null;
            }
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            float timer = 0;

            fireBallSimul.SetActive(true);
            fireBallSimul.transform.localScale = Vector3.one * 10;

            rangeSimul.SetActive(true);
            rangeSimul.transform.localScale = Vector3.one * FBSData.range * 2;

            while (timer <= FBSData.maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                fireBallSimul.transform.position = enemy.CenterPivot.transform.position + Vector3.ClampMagnitude(enemy.enemyStatus.EnemyTarget.CenterPivot.transform.position - enemy.CenterPivot.transform.position, FBSData.range);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        StopCoroutine("Simulation");
        fireBallSimul.SetActive(false);
        rangeSimul.SetActive(false);
    }

    public override void Exit()
    {
        StopCoroutine("Simulation");
        Fire();
    }

    void Fire()
    {
        Debug.Log("FireBall");

        if (user.tag == "Player")
        {
            Player player = user.GetComponent<Player>();

            GameObject Effect = Instantiate(FBSData.fireBallPrefab, fireBallSimul.transform.position,Quaternion.identity);
            HitDetection hitDetection = Effect.GetComponent<HitDetection>();

            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * FBSData.skillDefalutCoolTime;

            rangeSimul.SetActive(false);
            fireBallSimul.SetActive(false);

            Effect.tag = "PlayerAttack";
            Effect.layer = LayerMask.NameToLayer("PlayerAttack");

            hitDetection.SetHit_Ratio(FBSData.defaultDamage, FBSData.ratio, player.playerStats.SkillPower, FBSData.knockBack);
            hitDetection.SetSE(FBSData.BurnDeBuff);
            hitDetection.user = user;
            
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();

            GameObject Effect = Instantiate(FBSData.fireBallPrefab, fireBallSimul.transform.position, Quaternion.identity);
            HitDetection hitDetection = Effect.GetComponent<HitDetection>();

            skillCoolTime = 5;

            rangeSimul.SetActive(false);
            fireBallSimul.SetActive(false);

            Effect.tag = "EnemyAttack";
            Effect.layer = LayerMask.NameToLayer("EnemyAttack");

            hitDetection.SetHit_Ratio(FBSData.defaultDamage, FBSData.ratio, enemy.stats.SkillPower, FBSData.knockBack);
            hitDetection.SetSE(FBSData.BurnDeBuff);
            hitDetection.user = user;
        }
    }
}
