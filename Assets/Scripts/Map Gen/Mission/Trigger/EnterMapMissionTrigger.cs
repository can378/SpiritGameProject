using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMapMissionTrigger : MissionTriggerBase
{

    public override MISSION_TRIGGER_TYPE GetTriggerType()
    {
        return MISSION_TRIGGER_TYPE.EnterMap;
    }

    public override void SetTrigger()
    {
        print("entermap trigger µî·Ï");
        m_Owner.roomScript.AddLockEvent(m_Owner.StartMission);
    }
}

