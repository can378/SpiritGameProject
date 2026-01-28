using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatMinMaxUI :IStatUI
{
	Stat m_Stat;
	float m_Min;
	float m_Max;
	[field: SerializeField] TMP_Text m_StatName;
	[field: SerializeField] TMP_Text m_StatValue;
	[field: SerializeField] Image m_Fill;
	[field: SerializeField] bool m_Reverse;

	public override Stat GetStat() { return m_Stat; }
	public override void SetStat(Stat _Stat) 
	{
        // 기존에 구독 중인 스탯이 있다면 해제 (중요!)
        if (m_Stat != null)
            m_Stat.StatChangeEvent -= UpdateUI;

        m_Stat = _Stat;
        m_Min = m_Stat.Min;
        m_Max = m_Stat.Max;

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
		if(m_Reverse)
			m_Fill.fillAmount = (m_Max - m_Stat.Value) / (m_Max - m_Min);
		else
			m_Fill.fillAmount = (m_Stat.Value - m_Min) / (m_Max - m_Min);

		m_StatValue.text = (m_Stat.Value * 100.0f).ToString() + "%";
	}
}
