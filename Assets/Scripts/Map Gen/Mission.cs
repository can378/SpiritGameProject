using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType 
{KillAll, NoHurt, MiniBoss, TimeAttack, KillAll2, Curse,Dream,Maze,LittleMonster}

public class Mission : MonoBehaviour
{
    public MissionType type;
   

    public float timeCondition;


    private ObjectSpawn spawn;
    private float time;
    private float playerFirstHP;

    [Header("TimeAttack, Dream, LittleMonster")]
    public GameObject clock;

    [Header("Curse")]
    public GameObject curse;

    [Header("Maze")]
    public bool isEscapeMaze;


    [HideInInspector]
    public Room roomScript;


    private void Awake()
    {
        spawn = GetComponent<ObjectSpawn>();
        isEscapeMaze = false;
    }

    private void OnEnable()
    {
        time = 0;
        playerFirstHP = Player.instance.stats.HP;
        

        StartCoroutine(CheckMissionEnd());

        //Start clock
        if(MissionType.Dream == type || MissionType.LittleMonster==type || MissionType.TimeAttack==type) 
        { 
            StartCoroutine(clock.GetComponent<Clock>().ClockStart(timeCondition)); 
        }

        
    }


    //starts when the map is generated
    public IEnumerator CheckMissionEnd() 
    {
        time += Time.deltaTime;
        switch (type)
        {  
            case MissionType.NoHurt:
                //hurts --> fail
                if (Player.instance.stats.HP < playerFirstHP)
                { 
                    print("no hurt mission fail!!");
                    missionEnd();
                }
                //kill all of them --> end
                else if (KillAll()) 
                {
                    missionEnd();
                }
                break;
            case MissionType.KillAll:
            case MissionType.MiniBoss:
                if (KillAll()) 
                {
                    missionEnd();
                }
                break;
            case MissionType.KillAll2:
                if (time% 10==0)
                {
                    //respawn
                    spawn.SpawnEnemy(MapType.Mission);
                
                }
                if (KillAll())
                {
                    missionEnd();
                }
                break;
            case MissionType.Curse:

                if (KillAll()) 
                {
                    missionEnd();
                }
                else if(curse.transform.localScale.x>=0)
                { 
                    //curse safe area decreased
                    curse.transform.localScale-= new Vector3(0.001f, 0.001f, 0.001f);
                }
                break;
            case MissionType.TimeAttack:
                if (KillAll()) 
                {
                    missionEnd();
                }
                else if (time > timeCondition) 
                { 
                    print("time attack mission fail!!!");
                    missionEnd();
                }
                break;
            case MissionType.Dream:
                //ends over time
                if (time > timeCondition) 
                {
                    missionEnd();
                }
                break;
            case MissionType.LittleMonster:
                if (time > timeCondition)
                {
                    missionEnd();
                }
                break;
            case MissionType.Maze:
                if (isEscapeMaze) { missionEnd(); }
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
        { if (e.GetComponent<EnemyStats>().HP > 0) { return false; } }
        return true;
    }
    private void missionEnd() 
    {
        roomScript.UnLockDoor();
        StopCoroutine(CheckMissionEnd());
    }
}
