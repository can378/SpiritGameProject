using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossRoom : MonoBehaviour
{
    public GameObject floor;
    public GameObject door;
    public GameObject finalBoss1;
    public GameObject finalBoss2;
    public GameObject exit;

    private void Update()
    {
        if (finalBoss2.GetComponent<EnemyStats>().HP <= 0)
        { exit.SetActive(true); }
    }

    void startFinalBoss() 
    { 
        door.SetActive(true);
        finalBoss1.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            startFinalBoss();
        }
    }
}
