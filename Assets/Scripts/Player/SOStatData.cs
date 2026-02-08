using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스탯 레벨 정보
/// </summary>
[CreateAssetMenu(fileName = "NewStatData", menuName = "Player/Stat")]
public class SOStatData : ScriptableObject
{
    [field: SerializeField] public ActionDescription m_ActionDescription { get; private set; }
    // 스탯 
    [field: SerializeField] public Player.StatID m_StatID { get; private set; } = Player.StatID.END;
    // 아이콘
    [field: SerializeField] public Sprite m_Icon { get; private set; }
    // 증가 수치
    [field: SerializeField] public float m_Value { get; private set; }
    // 텍스트
    [field: SerializeField] public string m_StatName { get; private set; }

    public bool IsValid() { return m_StatID != Player.StatID.END; }

}
