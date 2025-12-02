using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/SpeedDe")]
public class SpeedDeBuff : BuffData
{
    // 배율
    // 이동속도, 공격속도 감소
    [field: SerializeField] public float decreasedMoveSpeed { get; set; }
    [field: SerializeField] public float decreasedAttackSpeed { get; set; }

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)      //지속시간 갱신
    {
        // 플레이어일 시
        if (_Buff.target.tag == "Player")
        {
            PlayerStats playerStats = _Buff.target.GetComponent<PlayerStats>();

            // 효과 잠시 제거
            playerStats.MoveSpeed.DecreasedValue -= _Buff.stack * decreasedMoveSpeed;
            playerStats.AttackSpeed.DecreasedValue -= _Buff.stack * decreasedAttackSpeed;

            // 중첩 
            _Buff.stack = _Buff.stack < DefaultMaxStack ? _Buff.stack + 1 : DefaultMaxStack;

            // 저항에 따른 지속시간 적용
            _Buff.curDuration = _Buff.duration = (1 - playerStats.SEResist[(int)buffType].Value) * defaultDuration;

            playerStats.MoveSpeed.DecreasedValue += _Buff.stack * decreasedMoveSpeed;
            playerStats.AttackSpeed.DecreasedValue += _Buff.stack * decreasedAttackSpeed;

        }
        // 그 외에
        else
        {
            Stats stats = _Buff.target.GetComponent<Stats>();

            // 효과 잠시 제거
            stats.MoveSpeed.DecreasedValue -= _Buff.stack * decreasedMoveSpeed;

            // 중첩 
            _Buff.AddStack();

            // 저항에 따른 지속시간 적용
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
            playerStats.AttackSpeed.DecreasedValue -= _Buff.stack * decreasedAttackSpeed;
        }
        else
        {
            Stats stats = _Buff.target.GetComponent<Stats>();

            stats.MoveSpeed.DecreasedValue -= _Buff.stack * decreasedMoveSpeed;

        }

    }
}