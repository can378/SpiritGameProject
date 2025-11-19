using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHurtMission : MissionBase
{
    ObjectBasic m_Target;
    

    public override MissionType GetMissionType()
    {
        return MissionType.NoHurt;
    }

    /// <summary>
    /// MissionController가 호출 시킨다.
    /// When Started Mission
    /// </summary>
    public override void StartMission()
    {
        m_Target = Player.instance;
        m_Target.BeAttackedEvent -= TargetAttacked;
        m_Target.BeAttackedEvent += TargetAttacked;
    }

    public void TargetAttacked()
    {
        m_Owner.FailedMission();
    }

    /// <summary>
    /// MissionController가 호출 시킨다.
    /// Progress Mission
    /// </summary>
    public override void CheckMission()
    {
        
    }

    /// <summary>
    /// MissionController가 호출 시킨다.
    /// When Ended Mission
    /// </summary>
    public override void EndMission()
    {
        m_Target.BeAttackedEvent -= TargetAttacked;
        m_Target = null;
    }
}
