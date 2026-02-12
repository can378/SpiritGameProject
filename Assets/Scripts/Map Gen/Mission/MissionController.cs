using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{ KillAll, NoHurt, MiniBoss, TimeAttack, KillAll2, Curse, Dream, Maze, LittleMonster, MazeCurse }

public enum MISSION_STATE { NotStarted, InProgress, Failed, Completed }
public enum MISSION_TRIGGER_TYPE { EnterMap, Interact, NONE }


/// <summary>
/// 미션 시작 시 몬스터 생성을 베이스로 미션 컨트롤러가 설계되어있음
/// </summary>
public class MissionController : MonoBehaviour
{
    [field: SerializeField] public MISSION_STATE missionState { get; private set; } = MISSION_STATE.NotStarted;
    [field: SerializeField] GameObject missionReward;
    public Room roomScript { get; private set; }

    [SerializeReference]
    public List<MissionBase> missions;
    public MissionTriggerBase missionTrigger;

    public MISSION_STATE GetMissionState()
    {
        return missionState;
    }

    // Trigger ?꽕?젙
    public void SetTrigger()
    {
        //print("Trigger");
        missionTrigger.SetTrigger();
    }

    public void SetRoom(Room _Room)
    {
        roomScript = _Room;
    }

    public void StartMission()
    {
        print("MissionStart");

        if (missionState != MISSION_STATE.NotStarted)
            return;

        foreach (MissionBase im in missions)
        {
            im.StartMission();
        }

        GetComponent<ObjectSpawn>().EnableEnemy();
        missionState = MISSION_STATE.InProgress;

        AudioManager.instance.OnEnterMap(gameObject);
    }

    void Update()
    {
        CheckMission();
    }

    void CheckMission()
    {
        if (missionState != MISSION_STATE.InProgress)
            return;

        foreach (MissionBase im in missions)
        {
            im.CheckMission();
        }
    }

    // End Mission
    // 다른 미션들에게도 알려준다.
    void EndMission()
    {
        if (missionState != MISSION_STATE.Completed && missionState != MISSION_STATE.Failed)
            return;

        foreach (MissionBase im in missions)
        {
            im.EndMission();
        }
    }

    // 미션 성공
    // 미션을 성공 처리하며 미션을 종료한다.
    public void CompletedMission()
    {
        missionState = MISSION_STATE.Completed;

        AudioManager.instance.OnExitMap(gameObject);
        roomScript.UnLockDoor();
        missionReward.SetActive(true);

        EndMission();
    }

    // 미션 실패
    // 미션을 실패 처리하며 미션을 종료한다.
    public void FailedMission()
    {
        missionState = MISSION_STATE.Failed;

        AudioManager.instance.OnExitMap(gameObject);
        roomScript.UnLockDoor();
        //missionReward.SetActive(true);

        EndMission();
    }
}
