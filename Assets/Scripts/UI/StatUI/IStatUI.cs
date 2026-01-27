using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public interface IStatUI
{
    public Stat GetStat();
    public TMP_Text GetName();
    public TMP_Text GetValue();
}
