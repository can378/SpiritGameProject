using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSword : Weapon
{
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }

    // ���� �ð�, ������, ����Ʈ
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

        // ��Ÿ�� ����
        coolTime = 2f;

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

            littleStar.tag = "PlayerAttack";
            littleStar.layer = LayerMask.NameToLayer("PlayerAttack");
            hitDetection.SetHitDetection(false, -1, false, -1, (defalutDamage + ratio * user.playerStats.skillPower) * 0.5f, knockBack, 0, 0, null);
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