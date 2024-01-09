using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    RoomManager roomManager;
    public Transform[] enemySpawnPoint;
    public List<GameObject> enemys;

    public void spawnEnemy(MapType mapType)
    {
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        this.transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, roomManager.roomSize);
        int ran; 
        if (mapType == MapType.Default)
        {
            foreach(Transform enemyTransform in enemySpawnPoint)
            {
                ran = Random.Range(0, roomManager.roomTemplates.normalEnemy.Length);
                enemys.Add(Instantiate(roomManager.roomTemplates.normalEnemy[ran], enemyTransform.position, enemyTransform.rotation));
            }
        }
        else if (mapType == MapType.MiniBoss)
        {
            foreach (Transform enemyTransform in enemySpawnPoint)
            {
                ran = Random.Range(0, roomManager.roomTemplates.miniBossEnemy.Length);
                enemys.Add(Instantiate(roomManager.roomTemplates.miniBossEnemy[ran], enemyTransform.position, enemyTransform.rotation));
            }
        }
        else if (mapType == MapType.Boss)
        {
            foreach (Transform enemyTransform in enemySpawnPoint)
            {
                ran = Random.Range(0, roomManager.roomTemplates.bossEnemy.Length);
                enemys.Add(Instantiate(roomManager.roomTemplates.bossEnemy[ran], enemyTransform.position, enemyTransform.rotation));
            }
        }
    }

    void OnDestroy() {
        foreach(GameObject enemy in enemys)
        {
            Destroy(enemy);
        }
        enemys.Clear();
    }

}
