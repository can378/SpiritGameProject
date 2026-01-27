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
        m_Stat = _Stat;
        m_Stat.StatChangeEvent += UpdateUI;
    }

    public override TMP_Text GetName() { return m_StatName; }
    public override TMP_Text GetValue() { return m_StatValue; }

    private void OnEnable()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        m_StatValue.text = m_Stat.Value.ToString();
    }
}
