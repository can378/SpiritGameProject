using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 인터페이스로 하고 싶지만 유니티가 인터페이스 인스펙터를 제공하지 않으므로 그냥 추상 MonBehaviour로 
public abstract class IStatUI : MonoBehaviour
{
    public abstract void SetStat(Stat _Stat);
    public abstract Stat GetStat();
    public abstract TMP_Text GetName();
    public abstract TMP_Text GetValue();

    public abstract void UpdateUI();

    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        if(GetStat() != null)
            GetStat().StatChangeEvent -= UpdateUI;
    }
}
