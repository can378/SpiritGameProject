using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType 
{KillAll, NoHurt, MiniBoss, TimeAttack, KillAll2, Curse, Dream, Maze, LittleMonster, MazeCurse}

public class Mission : MonoBehaviour
{
    public MissionType type;

    public GameObject missionReward;
    

    private ObjectSpawn spawn;
    [SerializeField][ReadOnly] private float time;
    private float playerFirstHP;
    [HideInInspector]
    public bool isStart=true;
    private bool isFail;                           //is player fail for mission
    [SerializeField] private bool isEnd;            //is mission ended
   

    [Header("TimeAttack, Dream, LittleMonster")]
    public float timeCondition;
    public GameObject clock;

    [Header("Curse, MazeCurse")]
    public GameObject curse;
    public GameObject safeAisle;
    public float curseDuration;
    public float curseTime;
    Vector3 curseDefaultScale;

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
        if (MissionType.NoHurt == type) { isStart = false; }
        else { isStart = true; }
        
        if(clock != null) { clock.SetActive(false); }
        
    }
    private void Start()
    {
        if (type == MissionType.MazeCurse)
        {
            //Debug.Log("start maze curse");
            startMission();
        }
    }
    //starts when the map is generated
    public IEnumerator CheckMissionEnd() 
    {
        if(isStart) time += Time.deltaTime;

        switch (type)
        {  
            case MissionType.NoHurt:
                //hurts --> fail
                if (FindObj.instance.Player.GetComponent<PlayerStats>().HP < playerFirstHP || 
                    time > timeCondition)
                { 
                    print("nohurt mission fail");
                    isFail = true;
                    clock.SetActive(false);
                    isEnd = true;

                }
                //kill all of them --> end
                else if (KillAll()) 
                {
                    print("no hurt mission success");
                    isEnd = true;
                }

                break;
            case MissionType.KillAll:
            case MissionType.MiniBoss:
                if (KillAll()) 
                {
                    isEnd = true;
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
                    isEnd = true;
                }
                break;
            case MissionType.Curse:

                if (KillAll()) 
                {
                    isEnd = true;
                }
                else if(curse.transform.localScale.x>=2)
                { 
                    //curse safe area decreased
                    curse.transform.localScale-= new Vector3(0.001f, 0.001f, 0.001f);
                }
                break;
            case MissionType.TimeAttack:
                if (KillAll()) 
                {
                    isEnd = true;
                }
                else if (time > timeCondition) 
                { 
                    print("fail");
                    isFail = true;
                    isEnd = true;
                }
                break;
            case MissionType.Dream:
                //ends over time
                if (time > timeCondition) 
                {
                    isEnd = true;
                }
                break;
            case MissionType.LittleMonster:
                if (time > timeCondition)
                {
                    isEnd = true;
                }
                break;
            case MissionType.Maze:
                isEnd = true;
                //if (isEscapeMaze) { isEnd = true; }
                break;
            case MissionType.MazeCurse:
                //Debug.Log("mazeCurse");
                curseTime += Time.deltaTime;
                if (curseTime < curseDuration)
                {
                    //curse safe area decreased
                    float ratio = (curseDuration - curseTime) / curseDuration;
                    curse.transform.localScale = curseDefaultScale * (ratio);
                }
                break;
            default:
                break;
        }

        yield return null;

        if (isEnd)
        {
            if (!isFail && missionReward!=null)
            {
                missionReward.SetActive(true);
            }

            if(type== MissionType.Curse)
            {
                safeAisle.SetActive(true);
            }

            if (roomScript != null)
            {
                roomScript.UnLockDoor();
            }
            
        }
        else
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

        if (MissionType.MazeCurse == type || MissionType.Curse == type)
        {
            curseTime = 0.0f;
            curseDefaultScale = curse.transform.localScale;
        }


        StartCoroutine(CheckMissionEnd());

        //Start clock
        if (MissionType.Dream == type || MissionType.LittleMonster == type || MissionType.TimeAttack == type||MissionType.NoHurt==type)
        {
            clock.SetActive(true);
            StartCoroutine(clock.GetComponent<Clock>().ClockStart(timeCondition));
        }
    }
    private bool KillAll()
    {
        foreach (GameObject e in spawn.enemys)
        { if (e.GetComponent<EnemyStats>().HP > 0) { return false; } }
        return true;
    }

    // 미션맵 BGM 플레이 관련 함수
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.OnEnterMap(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.OnExitMap(gameObject);
        }
    }
}
