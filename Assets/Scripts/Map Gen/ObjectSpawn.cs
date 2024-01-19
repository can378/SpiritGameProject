using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject enemyGroup;
    RoomManager roomManager;
    public List<Transform> enemySpawnPoint;
    public List<GameObject> enemys;

    public void SpawnEnemy(MapType mapType)
    {
        int ran;
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        this.transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, roomManager.roomSize);

        foreach (Transform enemyTransform in enemySpawnPoint)
        {
            GameObject instEnemy = null;
            if (mapType == MapType.Default)
            {
                ran = Random.Range(0, roomManager.roomTemplates.normalEnemy.Length);
                instEnemy = Instantiate(roomManager.roomTemplates.normalEnemy[ran], enemyTransform.position, enemyTransform.rotation);
            }
            else if (mapType == MapType.MiniBoss)
            {
                ran = Random.Range(0, roomManager.roomTemplates.miniBossEnemy.Length);
                instEnemy = Instantiate(roomManager.roomTemplates.miniBossEnemy[ran], enemyTransform.position, enemyTransform.rotation);
            }
            else if (mapType == MapType.Boss)
            {
                ran = Random.Range(0, roomManager.roomTemplates.bossEnemy.Length);
                instEnemy = Instantiate(roomManager.roomTemplates.bossEnemy[ran], enemyTransform.position, enemyTransform.rotation);
            }
            // ��ġ ����
            Destroy(enemyTransform.gameObject);
            // �θ� ����
            instEnemy.transform.SetParent(enemyGroup.transform);
            // ����Ʈ�� �߰�
            enemys.Add(instEnemy);
        }
        // ��ġ ���� �ʱ�ȭ
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
