using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/SEResistPlus")]
public class SEResistPlus : PassiveData
{
    // 상태이상 저항
    // +%p
    [SerializeField] float variation;
    public override void Update_Passive(ObjectBasic _User)
    { }
    public override void Apply(ObjectBasic target)
    {
        Debug.Log("플레이어 상태이상 저항 +" + variation * 100 + "%p 증가");
        Stats stats = target.GetComponent<PlayerStats>();
        for (int i = 0; i < stats.SEResist.Length; i++)
        {
            stats.SEResist[i].AddValue += variation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic target)
    {
        Stats stats = target.GetComponent<PlayerStats>();
        for (int i = 0; i < stats.SEResist.Length; i++)
        {
            stats.SEResist[i].AddValue -= variation;
        }
    }
}
