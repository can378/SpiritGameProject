using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/DodgeSpeedPlus")]
public class DodgeSpeedPlus : PassiveData
{
    // 회피 시 추가 이동속도 증가
    // %
    // 회피 시간
    // %
    [SerializeField] float timeVariation;
    [SerializeField] float speedVariation;
    public override void Update_Passive(ObjectBasic _User)
    { }
    public override void Apply(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            Debug.Log("플레이어 회피 시 추가 이동속도 +" + speedVariation * 100 + "%p 증가");
            Debug.Log("플레이어 회피 시간 -" + timeVariation * 100 + "% 감소");
            PlayerStats playerStats = _User.GetComponent<PlayerStats>();
            playerStats.increasedDodgeSpeed += speedVariation;
            playerStats.decreasedDodgeTime += timeVariation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            PlayerStats playerStats = _User.GetComponent<PlayerStats>();
            playerStats.increasedDodgeSpeed -= speedVariation;
            playerStats.decreasedDodgeTime -= timeVariation;
        }
    }
}
