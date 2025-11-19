using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeAttackMission : MissionBase
{
    [SerializeField] bool m_IsEndure;

    [SerializeField] float m_TimeAttackDuration;
    [SerializeField] float m_TimeAttackTimer;

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
        m_TimeAttackTimer = m_TimeAttackDuration;
    }


    public override void StartMission()
    {
        clock.SetActive(true);
        StartCoroutine(clock.GetComponent<Clock>().ClockStart(m_TimeAttackDuration));
        ResetTimer();
    }

    public override void CheckMission()
    {
        if (m_Owner.missionState != MISSION_STATE.InProgress)
        {
            return;
        }

        m_TimeAttackTimer -= Time.deltaTime;

        if(m_TimeAttackTimer <= 0)
        {
            if(m_IsEndure)
            {
                m_Owner.CompletedMission();
            }
            else
            {
                m_Owner.FailedMission();
                
            }
        }
    }

    public override void EndMission()
    {
        clock.SetActive(false);
        
    }
}
