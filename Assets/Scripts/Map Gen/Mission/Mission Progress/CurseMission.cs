using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurseMission : MissionBase
{

    [Header("Curse, MazeCurse")]
    public GameObject curse;
    public GameObject safeArea;
    public float curseDuration;
    public float curseTime;
    Vector3 curseDefaultScale;
    public ParticleSystem m_PS;




    public MissionController GetMissionController()
    {
        return m_Owner;
    }

    public override MissionType GetMissionType()
    {
        return MissionType.Curse;
    }


    public override void StartMission()
    {
        curse.SetActive(true);
        safeArea.SetActive(true);
        curseTime = curseDuration;
        curseDefaultScale = curse.transform.localScale;


        var shape = m_PS.shape;
        Room room = m_Owner.roomScript;
        shape.scale = new Vector3(room.transform.localScale.x, room.transform.localScale.y, 1.0f);
    }

    public override void CheckMission()
    {
        if (m_Owner.missionState != MISSION_STATE.InProgress)
        {
            return;
        }

        curseTime = curseTime - Time.deltaTime <= 0 ? 0 : curseTime - Time.deltaTime;

        // Á¤±ÔÈ­
        float normal = curseTime / curseDuration; 

        if (0 < curseTime)
        {
            //curse safe area decreased
            safeArea.transform.localScale = normal * curseDefaultScale;
        }
    }

    public override void EndMission()
    {
        curse.SetActive(false);
        safeArea.SetActive(false);
    }
}
