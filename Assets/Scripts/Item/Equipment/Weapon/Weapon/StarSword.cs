using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSword : Weapon
{
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }

    // 유지 시간, 프리팹, 이펙트
    [field: SerializeField] GameObject littleStarOrbitPrefab;

    float coolTime;
    GameObject effect;

    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null)
            return;

        if (effect != null)
            return;

        if(coolTime > 0f)
        {
            coolTime -= Time.deltaTime;
            return;
        }

        LittleStarOrbit littleStarOrbit;

        // 쿨타임 적용
        coolTime = 2f;

        // 이펙트 생성
        if (effect)
            Destroy(effect);
        effect = Instantiate(littleStarOrbitPrefab, user.CenterPivot.transform.position, user.CenterPivot.transform.rotation);
        effect.transform.localScale = new Vector3(1, 1, 1);

        // 공전 설정
        littleStarOrbit = effect.GetComponent<LittleStarOrbit>();
        littleStarOrbit.user = user.GetComponent<ObjectBasic>();
        littleStarOrbit.DefaultDamage = defalutDamage;
        littleStarOrbit.Ratio = ratio;

        foreach (GameObject littleStar in littleStarOrbit.littleStars)
        {
            HitDetection hitDetection = littleStar.GetComponent<HitDetection>();

            littleStar.tag = "PlayerAttack";
            littleStar.layer = LayerMask.NameToLayer("PlayerAttack");
            hitDetection.SetHit_Ratio(10, 0.1f, user.playerStats.SkillPower);
        }

    }

    public override void Equip(Player user)
    {
        base.Equip(user);
    }

    public override void UnEquip(Player user)
    {
        base.UnEquip(user);
        Destroy(effect);
    }
}
