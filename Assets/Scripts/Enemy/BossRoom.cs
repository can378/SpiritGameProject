using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameObject nextChapterDoor;
    GameObject boss;
    ObjectSpawn objectSpwan;

    // Start is called before the first frame update
    void Start()
    {
        objectSpwan = GetComponent<ObjectSpawn>();
        boss = objectSpwan.enemys[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.GetComponent<EnemyStats>().HP <= 0)
        { nextChapterDoor.SetActive(true); }
    }
}
