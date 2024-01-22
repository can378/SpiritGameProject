using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject enemyGroup;
    public List<Transform> enemySpawnPoint;
    public List<GameObject> enemys;

    EnemyTemplates enemyTemplates;
    RoomManager roomManager;

    public void SpawnEnemy(MapType mapType)
    {
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        enemyTemplates = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<EnemyTemplates>();
        this.transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, roomManager.roomSize);

        foreach (Transform enemyTransform in enemySpawnPoint)
        {
            int ran;
            GameObject instEnemy = null;
            if (mapType == MapType.Default)
            {
                ran = Random.Range(0, enemyTemplates.normalEnemy.Length);
                instEnemy = Instantiate(enemyTemplates.normalEnemy[ran], enemyTransform.position, enemyTransform.rotation);
            }
            else if (mapType == MapType.Boss)
            {
                ran = Random.Range(0, enemyTemplates.bossEnemy.Length);
                instEnemy = Instantiate(enemyTemplates.bossEnemy[ran], enemyTransform.position, enemyTransform.rotation);
            }
            else if (mapType == MapType.Mission)
            {

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
