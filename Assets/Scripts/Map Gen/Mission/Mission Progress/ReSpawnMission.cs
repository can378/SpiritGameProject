using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 제한 시간이 지난다면 해당 방의 모든 적을 부활시킨다.
[System.Serializable]
public class ReSpawnMission : MissionBase
{
    ObjectSpawn m_Spawner;
    [SerializeField] float m_RespawnDuration;
    [SerializeField] float m_RespawnTimer;

    [SerializeField] GameObject clock;

    public MissionController GetMissionController()
    {
        return m_Owner;
    }

    public override MissionType GetMissionType()
    {
        return MissionType.KillAll;
    }

    public void ResetTimer()
    {
        StartCoroutine(clock.GetComponent<Clock>().ClockStart(m_RespawnDuration));
        m_RespawnTimer = m_RespawnDuration;
    }


    public override void StartMission()
    {
        m_Spawner = m_Owner.GetComponent<ObjectSpawn>();
        clock.SetActive(true);
        ResetTimer();
    }

    public override void CheckMission()
    {
        if (m_Owner.missionState != MISSION_STATE.InProgress)
        {
            return;
        }

        m_RespawnTimer -= Time.deltaTime;

        if(m_RespawnTimer <= 0)
        {
            m_Spawner.RespawnEnemy();
            ResetTimer();
        }
    }

    public override void EndMission()
    {
        clock.SetActive(false);
    }
}
