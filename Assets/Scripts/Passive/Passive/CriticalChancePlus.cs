using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/CriticalChancePlus")]
public class CriticalChancePlus : PassiveData
{
    // 치명타 확률 증가
    // +%p

    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 치명타확률 +" + variation * 100 + "%p 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.CriticalChance.AddValue += variation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.CriticalChance.AddValue -= variation;
        }
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, variation * 100);
    }
}
