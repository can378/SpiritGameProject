using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMissionTrigger : MissionTriggerBase
{
    public Interactable m_Interactable;

    public override MISSION_TRIGGER_TYPE GetTriggerType()
    {
        return MISSION_TRIGGER_TYPE.Interact;
    }

    public override void SetTrigger()
    {
        m_Interactable.AddInteractEvent(m_Owner.StartMission);
    }
}


