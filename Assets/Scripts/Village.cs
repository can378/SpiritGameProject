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
            //ī�޶� �̵�
            CameraManager.instance.CameraMove(Entrance);
            CameraManager.instance.isCameraChasing = true;
            GameManager.instance.touchedObject = null;
            //�÷��̾� �̵�����

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (this.gameObject == Entrance) 
            { 
                //ī�޶� �̵�
                CameraManager.instance.isCameraChasing = false;
                CameraManager.instance.CameraMove(Inside);

                //�÷��̾� �̵� �Ұ�
            }

            //���� ���� ����
            if (this.gameObject == Cave) 
            { SceneManager.LoadScene("Map"); }

        }
    
    }




}
