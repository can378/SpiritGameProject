using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject RoomManagerObj;
    private List<GameObject> RoomList;
    public List<GameObject> EnemyPrefab;

    public static EnemySpawn instance = null;
    void Start()
    {
        instance = this;
        
    }


    public void spawnEnemy() 
    {
        RoomList = RoomManagerObj.GetComponent<RoomManager>().rooms;

        for (int i = 0; i < EnemyPrefab.Count; i++)
        {
            for (int j = 0; j < RoomList.Count; j++)
            {
                GameObject em = Instantiate(EnemyPrefab[i]);
                em.transform.position = RoomList[j].transform.position;
                em.transform.parent = this.transform;
            }
        }

    }


    //PATTERN========================================
}
