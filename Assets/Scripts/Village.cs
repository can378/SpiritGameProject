using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Village : MonoBehaviour
{
    public GameObject Inside;
    public GameObject Entrance;
    public GameObject Exit;

    private GameObject Player;
    
    void Start()
    {
        Player = FindObj.instance.Player;
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
            Player.GetComponent<PlayerStatus>().isPlayerMove = true;
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
                Player.GetComponent<PlayerStatus>().isPlayerMove = false;

            }

        }
    
    }




}
