using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeAttackMission : MissionBase
{
    [SerializeField] bool m_IsEndure;

    [SerializeField] float m_TimeAttackDuration;
    [SerializeField] float m_TimeAttackTimer;

    [SerializeField] GameObject m_Clock;



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
        StartCoroutine(m_Clock.GetComponent<Clock>().ClockStart(m_TimeAttackDuration));
        m_TimeAttackTimer = m_TimeAttackDuration;
    }


    public override void StartMission()
    {
        m_Clock.gameObject.SetActive(true);
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
        m_Clock.gameObject.SetActive(true);
        
    }
}
