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
            // ��ġ ����
            Destroy(enemyTransform.gameObject);
            // �θ� ����
            instEnemy.transform.SetParent(enemyGroup.transform);
            // ����Ʈ�� �߰�
            enemys.Add(instEnemy);
        }
        // ��ġ ���� �ʱ�ȭ
        // �� ���ֵ� �ǳ�?
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
