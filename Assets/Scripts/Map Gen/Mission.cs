using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType {KillAll, NoHurt, MiniBoss, TimeAttack,Jump,Lava,Dream}

public class Mission : MonoBehaviour
{
    public MissionType type;
   

    public float timeCondition;
    public List<GameObject> Lavas;


    private ObjectSpawn spawn;
    private float time;
    private float playerFirstHP;
    public GameObject clock;

    [HideInInspector]
    public Room roomScript;

    private void Awake()
    {
        spawn = GetComponent<ObjectSpawn>();
    }

    private void OnEnable()
    {
        time = 0;
        playerFirstHP = DataManager.instance.userData.playerHP;
        StartCoroutine(CheckMissionEnd());

        if(MissionType.Dream == type) 
        { 
            StartCoroutine(clock.GetComponent<Clock>().ClockStart(timeCondition)); 
            
        }
    }


    //starts when the map is generated
    public IEnumerator CheckMissionEnd() 
    {
        
        switch (type)
        {  
            case MissionType.NoHurt:
                //hurts --> fail
                if (DataManager.instance.userData.playerHP < playerFirstHP)
                { 
                    print("no hurt mission fail!!"); 
                    roomScript.UnLockDoor();
                    StopCoroutine(CheckMissionEnd());
                }
                //kill all of them --> end
                if (KillAll()) 
                { 
                    roomScript.UnLockDoor();  
                    StopCoroutine(CheckMissionEnd()); 
                }
                break;
            case MissionType.KillAll:
            case MissionType.MiniBoss:
                if (KillAll()) 
                { 
                    roomScript.UnLockDoor();
                    StopCoroutine(CheckMissionEnd());
                }
                break;
            case MissionType.Jump:
                //temp..
                break;
            case MissionType.Lava:
                //kill all of them --> end
                if (KillAll()) 
                { 
                    roomScript.UnLockDoor();
                    StopCoroutine(CheckMissionEnd());
                }
                else
                { 
                    //lava rises
                    foreach (GameObject lava in Lavas)
                    { lava.transform.localScale += new Vector3(0.001f,0.001f,0.001f); }
                }
                break;
            case MissionType.TimeAttack:
                time += Time.deltaTime;
                if (KillAll()) 
                { roomScript.UnLockDoor();
                    StopCoroutine(CheckMissionEnd());
                }
                if (time > timeCondition&&KillAll()==false) 
                { 
                    print("time attack mission fail!!!");
                    roomScript.UnLockDoor();
                    StopCoroutine(CheckMissionEnd());
                }
                break;
            case MissionType.Dream:
                //ends over time
                time += Time.deltaTime;
                if (time > timeCondition) 
                { 
                    roomScript.UnLockDoor();
                    StopCoroutine(CheckMissionEnd());
                }
                break;
            default:
                break;
        }

        yield return null;
        StartCoroutine(CheckMissionEnd());
    }

    private bool KillAll()
    {
        foreach (GameObject e in spawn.enemys)
        { if (e.GetComponent<EnemyStatus>().health > 0) { return false; } }
        return true;
    }

}
