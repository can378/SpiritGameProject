using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/CriticalChanceFull")]
public class CriticalChanceFull : PassiveData
{
    // 치명타 확률 증가
    // %p
    // 치명타 피해량 감소
    // %p

    [SerializeField] float chanceVariation;
    [SerializeField] float damageVariation;

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            Debug.Log("플레이어 치명타 확률 +" + chanceVariation * 100 + "%p 증가");
            Debug.Log("플레이어 치명타 피해량 +" + damageVariation * 100 + "%p 감소");
            PlayerStats playerStats = _User.GetComponent<PlayerStats>();
            playerStats.CriticalChance.AddValue += chanceVariation;
            playerStats.CriticalDamage.AddValue -= damageVariation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            PlayerStats playerStats = _User.GetComponent<PlayerStats>();
            playerStats.CriticalChance.AddValue -= chanceVariation;
            playerStats.CriticalDamage.AddValue += damageVariation;
        }
    }
}
