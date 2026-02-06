using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/SkillCoolTimeMinus")]
public class SkillCoolTimeMinus : PassiveData
{
    // 도술 대기 시간 감소
    // -%p
    [SerializeField] float variation;
    public override void Update_Passive(ObjectBasic _User)
    { }
    public override void Apply(ObjectBasic target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 도술 재사용 대기시간 +" + variation * 100 + "% 감소");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.SkillCoolTime.AddValue -= variation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.SkillCoolTime.AddValue += variation;
        }
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, variation * 100);
    }
}
