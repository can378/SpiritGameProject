using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillAllMission : MissionBase
{
    ObjectSpawn m_Spawner;


    public MissionController GetMissionController()
    {
        return m_Owner;
    }

    public override MissionType GetMissionType()
    {
        return MissionType.KillAll;
    }


    public override void StartMission()
    {
        /*
        if (m_Owner.missionState != MISSION_STATE.NotStarted)
        {
            roomScript.UnLockDoor();
            return;
        }

        missionState = MISSION_STATE.InProgress;
        AudioManager.instance.OnEnterMap(gameObject);
        */

        m_Spawner = m_Owner.GetComponent<ObjectSpawn>();

    }

    public override void CheckMission()
    {
        if (m_Owner.missionState != MISSION_STATE.InProgress)
        {
            return;
        }

        //print("Cheking...");

        if (KillAll())
        {
            m_Owner.CompletedMission();
        }
    }

    private bool KillAll()
    {
        foreach (GameObject e in m_Spawner.enemys)
        { if (!e.GetComponent<Status>().isDead) { return false; } }
        return true;
    }

    public override void EndMission()
    {
        
    }
}
