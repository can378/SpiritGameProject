using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/AttackSpeedUp")]
public class AttackSpeedUp : PassiveData
{
    // 공격속도 증가
    // +%

    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            Debug.Log("플레이어 공격속도 +" + variation * 100 + "% 증가");
            PlayerStats plyaerStats = _User.GetComponent<PlayerStats>();
            plyaerStats.AttackSpeed.IncreasedValue += variation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            PlayerStats plyaerStats = _User.GetComponent<PlayerStats>();
            plyaerStats.AttackSpeed.IncreasedValue -= variation;
        }
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, variation * 100);
    }
}
