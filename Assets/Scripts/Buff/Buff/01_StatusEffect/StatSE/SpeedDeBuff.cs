using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/SpeedDe")]
public class SpeedDeBuff : BuffData
{
    // ����
    // �̵��ӵ�, ���ݼӵ� ����
    [field: SerializeField] public float decreasedMoveSpeed { get; set; }
    [field: SerializeField] public float decreasedAttackSpeed { get; set; }

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)      //���ӽð� ����
    {
        // �÷��̾��� ��
        if (_Buff.target.tag == "Player")
        {
            PlayerStats playerStats = _Buff.target.GetComponent<PlayerStats>();

            // ȿ�� ��� ����
            playerStats.MoveSpeed.DecreasedValue -= _Buff.stack * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= _Buff.stack * decreasedAttackSpeed;

            // ��ø 
            _Buff.stack = _Buff.stack < DefaultMaxStack ? _Buff.stack + 1 : DefaultMaxStack;

            // ���׿� ���� ���ӽð� ����
            _Buff.curDuration = _Buff.duration = (1 - playerStats.SEResist[(int)buffType].Value) * defaultDuration;

            playerStats.MoveSpeed.DecreasedValue += _Buff.stack * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed += _Buff.stack * decreasedAttackSpeed;

        }
        // �� �ܿ�
        else
        {
            Stats stats = _Buff.target.GetComponent<Stats>();

            // ȿ�� ��� ����
            stats.MoveSpeed.DecreasedValue -= _Buff.stack * decreasedMoveSpeed;

            // ��ø 
            _Buff.AddStack();

            // ���׿� ���� ���ӽð� ����
            _Buff.curDuration = _Buff.duration = (1 - stats.SEResist[(int)buffType].Value) * defaultDuration;

            stats.MoveSpeed.DecreasedValue += _Buff.stack * decreasedMoveSpeed;

        }
    }

    public override void Update_Buff(Buff _Buff)
    {

    }

    public override void Remove(Buff _Buff)
    {
        if (_Buff.target.tag == "Player")
        {
            PlayerStats playerStats = _Buff.target.GetComponent<PlayerStats>();

            playerStats.MoveSpeed.DecreasedValue -= _Buff.stack * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= _Buff.stack * decreasedAttackSpeed;
        }
        else
        {
            Stats stats = _Buff.target.GetComponent<Stats>();

            stats.MoveSpeed.DecreasedValue -= _Buff.stack * decreasedMoveSpeed;

        }

    }
}