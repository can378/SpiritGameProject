using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject enemyGroup;
    public List<Transform> enemySpawnPoint;
    public List<GameObject> enemys;

    EnemyTemplates enemyTemplates;
    RoomManager roomManager;
    private UserData userData;
    private Dictionary<string, GameObject[]> enemyTemplatesDic;
    private int nowChapter;

    // 몬스터 활동 중지
    public void DisableEnemy()
    {
        for (int i = enemys.Count - 1; i >= 0; i--)
        {
            if (enemys[i].GetComponent<Stats>().HP <= 0)
            {
                GameObject enemy = enemys[i];
                enemys.RemoveAt(i);
                Destroy(enemy);
            }
            else
            {
                enemys[i].GetComponent<EnemyBasic>().enemyStatus.isTargetNoThisRoom = true;
                enemys[i].GetComponent<EnemyBasic>().enemyStatus.isTargetNoThisRoomTime = 0;
            }
        }
    }

    // 몬스터 활동 시작
    public void EnableEnemy()
    {
        for (int i = enemys.Count - 1; i >= 0; i--)
        {
            enemys[i].GetComponent<EnemyBasic>().enemyStatus.isTargetNoThisRoom = false;
            enemys[i].GetComponent<EnemyBasic>().enemyStatus.isTargetNoThisRoomTime = 0;
            enemys[i].SetActive(true);
        }
    }


    public void SpawnEnemy(MapType mapType)
    {
        userData = DataManager.instance.userData;
        roomManager = FindObj.instance.roomManagerScript;
        enemyTemplates = GameManager.instance.enemyTemplates;

        // enemyTemplates Dictionary
        enemyTemplatesDic = new Dictionary<string, GameObject[]>();
        enemyTemplatesDic.Add("normalEnemyCh1", enemyTemplates.normalEnemyCh1);
        enemyTemplatesDic.Add("normalEnemyCh2", enemyTemplates.normalEnemyCh2);
        enemyTemplatesDic.Add("normalEnemyCh3", enemyTemplates.normalEnemyCh3);


        nowChapter = userData.nowChapter;


        

        int spawnChapter = Mathf.Clamp(userData.nowChapter, 1, 3);
        foreach (Transform enemyTransform in enemySpawnPoint)
        {
            int ran;
            GameObject instEnemy = null;
            if (mapType == MapType.Default|| mapType==MapType.Mission)
            {
                if (Random.Range(0, 6) ==0)
                {
                    string key = "normalEnemyCh" + spawnChapter.ToString();

                    if (enemyTemplatesDic.TryGetValue(key, out GameObject[] templateArray) && templateArray.Length > 0)
                    {
                        ran = Random.Range(0, templateArray.Length);
                        //Debug.Log("normalEnemyCh" + nowChapter.ToString());
                        //Debug.Log(enemyTemplatesDic[key][ran].name);
                        instEnemy = Instantiate(templateArray[ran], enemyTransform.position, enemyTransform.rotation);
                    }
                    else { Debug.LogWarning($"Enemy template missing or empty for chapter {spawnChapter}: {key}"); }
                }

                if (instEnemy == null && enemyTemplates.normalEnemy != null && enemyTemplates.normalEnemy.Length > 0)
                {
                    ran = Random.Range(0, enemyTemplates.normalEnemy.Length);
                    instEnemy = Instantiate(enemyTemplates.normalEnemy[ran], enemyTransform.position, enemyTransform.rotation);
                }
                
            }
            else if (mapType == MapType.Boss)
            {
                GameObject[] bossArray = enemyTemplates.bossEnemy;
                if (bossArray != null && bossArray.Length > 0)
                {
                    int bossIndex = Mathf.Clamp(nowChapter - 1, 0, bossArray.Length - 1);
                    instEnemy = Instantiate(bossArray[bossIndex], enemyTransform.position, enemyTransform.rotation);
                }
                else
                {
                    Debug.LogWarning("Boss enemy template array is missing or empty.");
                }
            }

            // 위치 삭제
            Destroy(enemyTransform.gameObject);

            if (instEnemy == null)
                continue;

            // 부모 설정
            instEnemy.transform.SetParent(enemyGroup.transform);

            if(instEnemy.GetComponent<EnemyBasic>() != null)
            {
                instEnemy.GetComponent<EnemyBasic>().SetSummonFunc(Summon);
            }

            // 리스트에 추가
            enemys.Add(instEnemy);
        }
        // 위치 정보 초기화
        // 안 없애도 되나?
        enemySpawnPoint.Clear();

        foreach (GameObject enemy in enemys)
        {
            enemy.SetActive(false);
        }
        
    }

    public void RespawnEnemy()
    {
        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<EnemyBasic>().Revive(1.0f);
        }
        EnableEnemy();
    }

    void OnDestroy() {
        foreach(GameObject enemy in enemys)
        {
            Destroy(enemy);
        }
        enemys.Clear();
    }

    // if Enemy is Dead, Destroy Enemy
    void OnDisable() {
        for (int i = enemys.Count - 1; i >= 0; i--)
        {
            if (enemys[i].GetComponent<Stats>().HP <= 0)
            {
                GameObject enemy = enemys[i];
                enemys.RemoveAt(i);
                Destroy(enemy);
            }
        }
    }

    // 적이 적을 소환
    public GameObject Summon(EnemyBasic User, GameObject Enemy, Transform Trans)
    {
        // 1. 단순 확인
        if (Enemy.GetComponent<EnemyBasic>() == null)
        {
            Debug.Log("EnemyBasic component found on the Enemy GameObject.");
            return null;
        }

        // 몬스터 생성
        GameObject instEnemy = Instantiate(Enemy, Trans.position, Trans.rotation);

        // 부모 설정
        instEnemy.transform.SetParent(enemyGroup.transform);

        // 리스트에 추가
        enemys.Add(instEnemy);
        User.AddSummon(instEnemy);

        return instEnemy;

    }
}
