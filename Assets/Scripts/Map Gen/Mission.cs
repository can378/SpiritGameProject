using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType {KillAll, NoHurt, MiniBoss, TimeAttack,Jump,Lava,Dream}

public class Mission : MonoBehaviour
{
    public MissionType type;

    private void Update()
    {
       
    }

    //starts when the map is generated
    public IEnumerator CheckMissionEnd() 
    {

        switch (type)
        {
            case MissionType.KillAll:
                break;
            case MissionType.NoHurt:
                break;
            case MissionType.MiniBoss:
                break;
            case MissionType.TimeAttack:
                break;
            case MissionType.Jump:
                break;
            case MissionType.Lava:
                break;
            case MissionType.Dream:
                break;
            default:
                break;
        }
        yield return null;
        StartCoroutine(CheckMissionEnd());
    }

    private void CheckKillAll()
    {
        
    }

}
