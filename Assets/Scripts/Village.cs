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

    void Start()
    {
    }


    void Update()
    {
        MouseClick();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (this.gameObject == Entrance) 
            { 
                //카메라 이동
                CameraManager.instance.view3rd = false;
                CameraManager.instance.CameraMove(Inside);
            }

            //메인 게임 시작
            if (this.gameObject == Cave) 
            { SceneManager.LoadScene("Map"); }

        }
    
    }



    void MouseClick()
    {

        if (Input.GetMouseButton(0))
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPos, Camera.main.transform.forward);

            if (hit.collider != null)
            {
                GameObject touchedObject = hit.transform.gameObject;
                if (touchedObject == Exit) 
                {
                    //카메라 이동
                    CameraManager.instance.CameraMove(Entrance);
                    CameraManager.instance.view3rd = true;

                }
            }
        }

    }



}
