using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/DodgeTimeUp")]
public class DodgeTimeUp : PassiveData
{
    // 회피 시 추가 이동속도 감소
    // %
    // 회피 시간
    // %
    [SerializeField] float timeVariation;
    [SerializeField] float speedVariation;

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            Debug.Log("플레이어 회피 시 추가 이동속도 +" + speedVariation * 100 + "%p 증가");
            Debug.Log("플레이어 회피 시간 +" + timeVariation * 100 + "% 증가");
            PlayerStats playerStats = _User.GetComponent<PlayerStats>();
            playerStats.decreasedDodgeSpeed += speedVariation;
            playerStats.increasedDodgeTime += timeVariation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            PlayerStats playerStats = _User.GetComponent<PlayerStats>();
            playerStats.decreasedDodgeSpeed -= speedVariation;
            playerStats.increasedDodgeTime -= timeVariation;
        }
    }
}
