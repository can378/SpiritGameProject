using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatMinMaxUI : MonoBehaviour, IStatUI
{

    [field: SerializeField] Stat m_Stat;
    [field: SerializeField] TMP_Text m_Name;
    [field: SerializeField] TMP_Text m_Value;

    public Stat GetStat() { return m_Stat; }
    public TMP_Text GetName() { return m_Name; }
    public TMP_Text GetValue() { return m_Value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
