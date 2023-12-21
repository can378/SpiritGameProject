using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Village : MonoBehaviour
{
    public GameObject Inside;
    public GameObject Entrance;
    public GameObject Exit;
    public GameObject Cave;

    public GameObject Player;
    
    void Start()
    {
    }


    void Update()
    {
        if (GameManager.instance.touchedObject == Exit)
        {
            //카메라 이동
            CameraManager.instance.CameraMove(Entrance);
            CameraManager.instance.isCameraChasing = true;
            GameManager.instance.touchedObject = null;
            //플레이어 이동가능

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (this.gameObject == Entrance) 
            { 
                //카메라 이동
                CameraManager.instance.isCameraChasing = false;
                CameraManager.instance.CameraMove(Inside);

                //플레이어 이동 불가
            }

            //메인 게임 시작
            if (this.gameObject == Cave) 
            { SceneManager.LoadScene("Map"); }

        }
    
    }




}
