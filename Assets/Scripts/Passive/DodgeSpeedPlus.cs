using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/DodgeSpeedPlus")]
public class DodgeSpeedPlus : PassiveData
{
    // ȸ�� �� �߰� �̵��ӵ� ����
    // %
    // ȸ�� �ð�
    // %
    [SerializeField] float timeVariation;
    [SerializeField] float speedVariation;
    public override void Update_Passive(ObjectBasic _User)
    { }
    public override void Apply(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            Debug.Log("�÷��̾� ȸ�� �� �߰� �̵��ӵ� +" + speedVariation * 100 + "%p ����");
            Debug.Log("�÷��̾� ȸ�� �ð� -" + timeVariation * 100 + "% ����");
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
