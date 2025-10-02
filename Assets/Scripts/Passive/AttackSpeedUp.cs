using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/AttackSpeedUp")]
public class AttackSpeedUp : PassiveData
{
    // ���ݼӵ� ����
    // +%

    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            Debug.Log("�÷��̾� ���ݼӵ� +" + variation * 100 + "% ����");
            PlayerStats plyaerStats = _User.GetComponent<PlayerStats>();
            plyaerStats.increasedAttackSpeed += variation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            PlayerStats plyaerStats = _User.GetComponent<PlayerStats>();
            plyaerStats.increasedAttackSpeed -= variation;
        }
    }
}
