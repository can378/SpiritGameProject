using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class StatValueUI : IStatUI
{
    Stat m_Stat;
    [field: SerializeField] TMP_Text m_StatName;
    [field: SerializeField] TMP_Text m_StatValue;

    public override Stat GetStat() { return m_Stat; }
    public override void SetStat(Stat _Stat)
    {
        // 기존에 구독 중인 스탯이 있다면 해제 (중요!)
        if (m_Stat != null)
            m_Stat.StatChangeEvent -= UpdateUI;

        m_Stat = _Stat;

        if (m_Stat != null)
        {
            m_Stat.StatChangeEvent += UpdateUI;
            UpdateUI(); // 연결 즉시 초기화
        }
    }

    public override TMP_Text GetName() { return m_StatName; }
    public override TMP_Text GetValue() { return m_StatValue; }

    public override void UpdateUI()
    {
        //Debug.Log(m_Stat);
        m_StatValue.text = m_Stat.Value.ToString();
    }
}
