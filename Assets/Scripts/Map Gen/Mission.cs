using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType 
{KillAll, NoHurt, MiniBoss, TimeAttack, KillAll2, Curse,Dream,Maze,LittleMonster}

public class Mission : MonoBehaviour
{
    public MissionType type;

    public GameObject missionReward;
    

    private ObjectSpawn spawn;
    private float time;
    private float playerFirstHP;
    private bool isFail;                            // 미션 실패
    [SerializeField] private bool isEnd;            // 미션 종료(실패하든 안하든 끝나면 체크)
   

    [Header("TimeAttack, Dream, LittleMonster")]
    public float timeCondition;
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
        isFail = false;
        isEnd = false;
    }

    private void OnEnable()
    {
        startMission();
    }

    //starts when the map is generated
    // 미션 종료하면 몬스터나 미로 입구 흔적 등 없애야 할 듯
    public IEnumerator CheckMissionEnd() 
    {
        time += Time.deltaTime;
        switch (type)
        {  
            case MissionType.NoHurt:
                //hurts --> fail
                if (FindObj.instance.Player.GetComponent<PlayerStats>().HP < playerFirstHP)
                { 
                    print("nohurt mission fail");
                    isFail = true;

                    foreach (GameObject e in spawn.enemys)
                    { 
                        e.GetComponent<EnemyStats>().HP = 0;
                        e.SetActive(false);
                        clock.SetActive(false);
                    }
                    missionEnd();

                }
                //kill all of them --> end
                else if (KillAll()) 
                {
                    print("no hurt mission success");
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
                    print("fail");
                    isFail = true;
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
    public void startMission() 
    {
        if(isEnd)
        {
            roomScript.UnLockDoor();
            return;
        }

        time = 0;
        playerFirstHP = FindObj.instance.Player.GetComponent<PlayerStats>().HP;


        StartCoroutine(CheckMissionEnd());

        //Start clock
        if (MissionType.Dream == type || MissionType.LittleMonster == type || MissionType.TimeAttack == type||MissionType.NoHurt==type)
        {
            StartCoroutine(clock.GetComponent<Clock>().ClockStart(timeCondition));
        }
    }
    private bool KillAll()
    {
        foreach (GameObject e in spawn.enemys)
        { if (e.GetComponent<EnemyStats>().HP > 0) { return false; } }
        return true;
    }
    private void missionEnd() 
    {
        if (!isFail)
        {
            missionReward.SetActive(true);
        }

        isEnd = true;
        roomScript.UnLockDoor();
        StopCoroutine(CheckMissionEnd());

    }
}
