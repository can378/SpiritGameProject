using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public bool bossDead = false;
    public GameObject nextChapterDoor;
    GameObject boss;
    ObjectSpawn objectSpwan;


    void Start()
    {
        objectSpwan = GetComponent<ObjectSpawn>();
        boss = objectSpwan.enemys[0];
    }


    void Update()
    {
        if (bossDead)
        { nextChapterDoor.SetActive(true); }

    }
}
