using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class IStatUI : MonoBehaviour
{
    public abstract Stat GetStat();
    public abstract void SetStat(Stat _Stat);
    public abstract TMP_Text GetName();
    public abstract TMP_Text GetValue();
}
