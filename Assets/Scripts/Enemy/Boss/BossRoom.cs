using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{

    /*
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
    */

    Room m_Room;

    Boss m_Boss;

    public void SetRoom(Room _Room)
    {
        m_Room = _Room;

        m_Room.LockEvent += TrapBossRoom;
    }

    // 문이 닫히면 실행되는 함수 모음
    void TrapBossRoom()
    {
        GameObject boss = m_Room.map.GetComponent<ObjectSpawn>().enemys[0];
        m_Boss = boss.GetComponent<Boss>();

        // 보스 움직일 수 있게
        m_Boss.enemyStatus.isTarget = true;
        m_Boss.BossCutScene();

        PlayBgm();
        ZoomInBoss();
    }

    void PlayBgm()
    {
        //start audio
        AudioManager.instance.Bgm_boss(DataManager.instance.userData.nowChapter);
    }

    void ZoomInBoss()
    {
        //camera moving
        CameraManager.instance.isCameraChasing = false;
        StartCoroutine(CameraManager.instance.BossRoomEnterEffect(m_Boss, m_Room.gameObject));
    }

    // 이거는 플레이어에서 만들어야 할 듯
    IEnumerator DontMovePlayer()
    {
        yield return null;
    }


}
