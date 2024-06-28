using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum mazePortal { enter,exit};
public class MazeEnter : MonoBehaviour
{
    public mazePortal mazePortal;

    public GameObject maze;
    //public Vector2 mazePos;

    private GameObject mazeInst=null;


    void Start()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (mazePortal == mazePortal.enter)
            {

                foreach (Transform child in GameObject.FindWithTag("roomParent").transform)
                {
                    child.gameObject.SetActive(false);
                }

                //generate maze
                mazeInst = Instantiate(maze);
                //mazeInst.transform.position = mazePos;
                //mazeInst.GetComponent<MazeGenerator>().GenerateMaze();
                //GameObject.FindWithTag("Maze").transform.position = mazePos;



                //player move
                //collision.transform.position = mazePos;
                collision.transform.position = new Vector2(0,0);
                
                //camera move
                CameraManager.instance.CameraMove(collision.gameObject);
                CameraManager.instance.CenterMove(collision.gameObject);
                CameraManager.instance.mapSize = new Vector2(25, 25);
            }
            else if(mazePortal==mazePortal.exit)
            {
                foreach (Transform child in GameObject.FindWithTag("roomParent").transform)
                {
                    child.gameObject.SetActive(true);
                }

                //entrance disabled
                
                //GameObject.FindWithTag("MazeEntrance").SetActive(false);
                //GetComponent<CircleCollider2D>().enabled = false;


                GameObject.FindWithTag("Maze").SetActive(false);
                GameObject.FindWithTag("MazeBgr").SetActive(false);


                //player move
                Vector3 backToMap = new Vector3(5, 0, 0);
                collision.transform.position = GameManager.instance.nowRoom.transform.position+ backToMap;


                //camera move
                CameraManager.instance.CameraMove(collision.gameObject);
                CameraManager.instance.CenterMove(collision.gameObject);
                CameraManager.instance.mapSize = new Vector2(15, 15);



            }
            
        }
    }
}
