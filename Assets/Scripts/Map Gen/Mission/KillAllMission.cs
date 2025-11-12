using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllMission : RoomMissionBase
{
    private ObjectSpawn spawn;

    private void Awake()
    {
        spawn = GetComponent<ObjectSpawn>();
    }

    private void Update()
    {
        CheckMissionEnd();
    }

    public override MissionType GetMissionType()
    {
        return MissionType.KillAll;
    }
    
    public override void LockDoor()
    {
        StartMission();
    }
    
    public override void StartMission()
    {
        if (missionState != MISSION_STATE.NotStarted)
        {
            roomScript.UnLockDoor();
            return;
        }

        missionState = MISSION_STATE.InProgress;
        AudioManager.instance.OnEnterMap(gameObject);
    }
    
    protected override void CheckMissionEnd()
    {
        if(missionState != MISSION_STATE.InProgress)
        {
            return;
        }

        if(KillAll())
        {
            EndMission();
        }
    }

    private bool KillAll()
    {
        foreach (GameObject e in spawn.enemys)
        { if (e.GetComponent<EnemyStats>().HP > 0) { return false; } }
        return true;
    }

    public override void EndMission()
    {
        missionState = MISSION_STATE.Completed;
        AudioManager.instance.OnExitMap(gameObject);
        roomScript.UnLockDoor();
        missionReward.SetActive(true);
    }
}
