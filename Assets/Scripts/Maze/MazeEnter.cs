using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum mazePortal { enter,exit};
public class MazeEnter : MonoBehaviour
{
    public mazePortal mazePortal;

    public GameObject maze;
    public Vector2 mazePos;

    private GameObject mazeInst=null;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (mazePortal == mazePortal.enter)
            { 
                //generate maze
                mazeInst = Instantiate(maze);
                mazeInst.transform.position = mazePos;
                mazeInst.GetComponent<MazeGenerator>().GenerateMaze();
                //GameObject.FindWithTag("Maze").transform.position = mazePos;

                

                //player move
                collision.transform.position = mazePos;
                
                //camera move
                CameraManager.instance.CameraMove(collision.gameObject);
                CameraManager.instance.CenterMove(collision.gameObject);
                CameraManager.instance.mapSize = new Vector2(25, 25);
            }
            else if(mazePortal==mazePortal.exit)
            {
                //entrance disabled
                GameObject.FindWithTag("MazeEntrance").GetComponent<CircleCollider2D>().enabled = false;

                //player move
                collision.transform.position = GameObject.FindWithTag("MazeEntrance").transform.position;

                //camera move
                CameraManager.instance.CameraMove(collision.gameObject);
                CameraManager.instance.CenterMove(collision.gameObject);
                CameraManager.instance.mapSize = new Vector2(15, 15);


                GameObject.FindWithTag("Maze").SetActive(false);
                GameObject.FindWithTag("MazeBgr").SetActive(false);

            }
            
        }
    }
}
