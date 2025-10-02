using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/LittleStar")]

public class LittleStarBuff : BuffData
{
    // 기본 대미지
    [field: SerializeField] public int defalutDamage { get; private set; }
    // 계수
    [field: SerializeField] public float ratio { get; private set; }
    // 프리팹
    [field: SerializeField] GameObject littleStarOrbitPrefab;

    public override void Apply(Buff _Buff)
    {
        _Buff.CustomData.Add("Effect", null);
        _Buff.CustomData.Add("Finish", false);
        Overlap(_Buff);
    }

    public override void Remove(Buff _Buff)
    {
        Destroy((GameObject)_Buff.CustomData["Effect"]);
    }

    public override void Update_Buff(Buff _Buff)
    {
        if (_Buff.target == null)
            return;

        GameObject Effect = (GameObject)_Buff.CustomData["Effect"];
        bool Finish = (bool)_Buff.CustomData["Finish"];

        if (!Finish && Effect == null)
        {
            _Buff.curDuration = 5;
            _Buff.CustomData["Finish"] = true;
        }
    }


    public override void Overlap(Buff _Buff)      //지속시간 갱신
    {
        // 중첩 
        _Buff.AddStack();

        Stats stats = _Buff.target.GetComponent<Stats>();
        _Buff.curDuration = _Buff.duration = defaultDuration;

        GameObject Effect = (GameObject)_Buff.CustomData["Effect"];
        LittleStarOrbit littleStarOrbit;

        // 이펙트 생성
        // 이펙트가 있다면 제거
        if (Effect)
            Destroy(Effect);
        Effect = Instantiate(littleStarOrbitPrefab, _Buff.target.CenterPivot.transform.position, _Buff.target.CenterPivot.transform.rotation);
        Effect.transform.localScale = new Vector3(1, 1, 1);

        // 공전 설정
        littleStarOrbit = Effect.GetComponent<LittleStarOrbit>();
        littleStarOrbit.user = _Buff.target.GetComponent<ObjectBasic>();
        littleStarOrbit.DefaultDamage = defalutDamage;
        littleStarOrbit.Ratio = ratio;

        foreach (GameObject littleStar in littleStarOrbit.littleStars)
        {
            HitDetection hitDetection = littleStar.GetComponent<HitDetection>();

            if (_Buff.target.tag == "Player")
            {
                littleStar.tag = "PlayerAttack";
                littleStar.layer = LayerMask.NameToLayer("PlayerAttack");
            }
            else if (_Buff.target.tag == "Enemy")
            {
                littleStar.tag = "EnemyAttack";
                littleStar.layer = LayerMask.NameToLayer("EnemyAttack");
            }
            hitDetection.SetHit_Ratio(10, 0.1f, _Buff.target.stats.SkillPower);
        }

        _Buff.CustomData["Effect"] = Effect;

    }
}
