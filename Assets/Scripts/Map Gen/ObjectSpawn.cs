using System.Collections;
using System.Collections.Generic;
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


        transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, roomManager.roomSize);

        foreach (Transform enemyTransform in enemySpawnPoint)
        {
            int ran;
            GameObject instEnemy = null;
            if (mapType == MapType.Default|| mapType==MapType.Mission)
            {
                if (Random.Range(0, 6) ==0) 
                {
                    string key = "normalEnemyCh" + nowChapter.ToString();

                    if (enemyTemplatesDic.ContainsKey(key))
                    {
                        ran = Random.Range(0, enemyTemplatesDic[key].Length);
                        instEnemy = Instantiate(enemyTemplatesDic[key][ran], enemyTransform.position, enemyTransform.rotation);
                    }
                    else { print("there is no key here"); }
                    
                }
                else 
                {
                    ran = Random.Range(0, enemyTemplates.normalEnemy.Length);
                    instEnemy = Instantiate(enemyTemplates.normalEnemy[ran], enemyTransform.position, enemyTransform.rotation);
                }
                
            }
            else if (mapType == MapType.Boss)
            {
                ran = nowChapter - 1;
                //ran = Random.Range(0, enemyTemplates.bossEnemy.Length);
                instEnemy = Instantiate(enemyTemplates.bossEnemy[ran], enemyTransform.position, enemyTransform.rotation);
                MapUIManager.instance.SetBossProgress(instEnemy.GetComponent<EnemyBasic>());
            }
            
            // 위치 삭제
            Destroy(enemyTransform.gameObject);
            // 부모 설정
            instEnemy.transform.SetParent(enemyGroup.transform);
            
            // 리스트에 추가
            enemys.Add(instEnemy);
        }
        // 위치 정보 초기화
        // 안 없애도 되나?
        enemySpawnPoint.Clear();
        
    }


    void OnDestroy() {
        foreach(GameObject enemy in enemys)
        {
            Destroy(enemy);
        }
        enemys.Clear();
    }

}
