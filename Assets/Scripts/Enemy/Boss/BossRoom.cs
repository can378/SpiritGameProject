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

    // 문이 닫히면 실행되는 함수 모음
    void EnterBossRoom() { }
    void ZoomInBoss() { }

    // 이거는 플레이어에서 만들어야 할 듯
    IEnumerator DontMovePlayer()
    {
        yield return null;
    }


}
