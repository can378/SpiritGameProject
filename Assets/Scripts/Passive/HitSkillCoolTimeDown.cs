using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/HitSkillCoolTimeDown")]
public class HitSkillCoolTimeDown : PassiveData
{
    // �÷��̾ hitTarget ������ variation��ŭ ���� ��ų ��Ÿ�� ����
    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User)
    {
        if (_User == null)
            return;

        Player player = _User.GetComponent<Player>();

        if (player == null)
            return;


        if (!player.playerStatus.hitTarget || player.playerStats.skill[player.playerStatus.skillIndex] == 0)
            return;

        if (player.playerStatus.hitTarget.gameObject.tag == "Wall" || player.playerStatus.hitTarget.gameObject.tag == "Door" || player.playerStatus.hitTarget.gameObject.tag == "ShabbyWall")
            return;

        player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillCoolTime -= variation;
    }

    public override void Apply(ObjectBasic _User)
    {

    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {

    }
}
