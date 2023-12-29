using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject RoomManagerObj;
    private List<GameObject> RoomList;
    public List<GameObject> EnemyPrefab;
    

    void Start()
    {
        RoomList = RoomManagerObj.GetComponent<RoomManager>().room;
    }

    private void Update()
    {
        if (RoomManagerObj.GetComponent<RoomManager>().generateFinish == true)
        {
            spawnEnemy();
            RoomManagerObj.GetComponent<RoomManager>().generateFinish = false;
        }//RoomManger���� �� ��ũ��Ʈ�� �����Ͽ� �Լ� �����Ű���� �ٲ� ����
       
    }

    void spawnEnemy() 
    {
        for (int i = 0; i < EnemyPrefab.Count; i++)
        {
            for (int j = 0; j < RoomList.Count; j++)
            {
                GameObject em = (GameObject)Instantiate(EnemyPrefab[i]);
                em.transform.position = RoomList[j].transform.position;
                em.transform.parent = this.transform;
            }
        }

    }


    //PATTERN========================================



}
