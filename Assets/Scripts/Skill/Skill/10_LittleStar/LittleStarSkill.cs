using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleStarSkill : Skill
{
    // ���ط�
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }
    [field: SerializeField] public float knockBack {get; private set;}

    // ���� �ð�, ������, ����Ʈ
    [field: SerializeField] float time;
    [field: SerializeField] GameObject littleStarOrbitPrefab;
    GameObject effect;

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        Summon();
    }

    public override void Cancle()
    {
        
    }

    public override void Exit()
    {

    }

    void Summon()
    {
        Debug.Log("LittleStar");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            LittleStarOrbit littleStarOrbit;

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            // ����Ʈ ����
            if(effect)
                Destroy(effect);
            effect = Instantiate(littleStarOrbitPrefab, user.transform.position, user.transform.rotation);
            effect.transform.localScale = new Vector3(1, 1, 1);

            // ���� ����
            littleStarOrbit = effect.GetComponent<LittleStarOrbit>();
            littleStarOrbit.user = user.GetComponent<ObjectBasic>();

            foreach (GameObject littleStar in littleStarOrbit.littleStars)
            {
                HitDetection hitDetection = littleStar.GetComponent<HitDetection>();

                littleStar.tag = "PlayerAttack";
                littleStar.layer = LayerMask.NameToLayer("PlayerAttack");
                hitDetection.SetHitDetection(false,-1,false,-1,(defalutDamage + ratio * player.playerStats.skillPower) * 0.5f, knockBack, 0,0,null);
            }

            // rate ���� ����
            Destroy(effect,time);
        }
        else if (user.tag == "Enemy")
        {

            ObjectBasic objectBasic = this.user.GetComponent<ObjectBasic>();
            LittleStarOrbit littleStarOrbit;

            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;

            // ����Ʈ ����
            if (effect)
                Destroy(effect);
            effect = Instantiate(littleStarOrbitPrefab, user.transform.position, user.transform.rotation);
            effect.transform.localScale = new Vector3(1, 1, 1);

            // ���� ����
            littleStarOrbit = effect.GetComponent<LittleStarOrbit>();
            littleStarOrbit.user = user.GetComponent<ObjectBasic>();

            foreach (GameObject littleStar in littleStarOrbit.littleStars)
            {
                HitDetection hitDetection = littleStar.GetComponent<HitDetection>();

                littleStar.tag = "EnemyAttack";
                littleStar.layer = LayerMask.NameToLayer("EnemyAttack");
                hitDetection.SetHitDetection(false, -1, false, -1, (defalutDamage) * 0.5f, knockBack, 0, 0, null);
            }
        }
    }

    
}
