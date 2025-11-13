using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        m_RespawnTimer = m_RespawnDuration;
    }


    public override void StartMission()
    {
        m_Spawner = m_Owner.GetComponent<ObjectSpawn>();
        clock.SetActive(true);
        StartCoroutine(clock.GetComponent<Clock>().ClockStart(m_RespawnDuration));
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
            StartCoroutine(clock.GetComponent<Clock>().ClockStart(m_RespawnDuration));
            ResetTimer();
        }
    }

    public override void EndMission()
    {
        clock.SetActive(false);
    }
}
