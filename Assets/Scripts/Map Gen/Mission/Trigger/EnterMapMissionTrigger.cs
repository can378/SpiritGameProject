using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnterMapMissionTrigger : MissionTriggerBase
{

    public override MISSION_TRIGGER_TYPE GetTriggerType()
    {
        return MISSION_TRIGGER_TYPE.EnterMap;
    }

    public override void SetTrigger()
    {
        //print("entermap trigger µî·Ï");
        
        m_Owner.roomScript.LockEvent -= m_Owner.StartMission;

        m_Owner.roomScript.LockEvent += m_Owner.StartMission;
    }

    void OnDestroy()
    {
        m_Owner.roomScript.LockEvent -= m_Owner.StartMission;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.OnEnterMap(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.OnExitMap(gameObject);
        }
    }

}

