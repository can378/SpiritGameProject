using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionTriggerBase : MonoBehaviour
{
    [field : SerializeField] public MissionController m_Owner { get; set; }


    void Awake()
    {
        m_Owner = GetComponent<MissionController>();
    }

    public abstract MISSION_TRIGGER_TYPE GetTriggerType();
    public abstract void SetTrigger();
}
