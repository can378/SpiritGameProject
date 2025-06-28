using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum mazePortal { enter,exit};
public class MazeEnter : MonoBehaviour
{
    public mazePortal mazePortal;

    public GameObject maze;//mazeBgr임
    //public Vector2 mazePos;

    private Vector2 originalCameraAreaSize;
    private GameObject mazeInstance = null;
    private GameObject roomParent=null;

    private void Awake()
    {
        originalCameraAreaSize = CameraManager.instance.mapSize;
    }


    private void OnTriggerEnter2D(Collider2D playerCollision)
    {
        if (playerCollision.CompareTag("Player"))
        {
            roomParent = GameObject.FindWithTag("roomParent");

            if (mazePortal == mazePortal.enter)
            {
                // rooms deactivate
                foreach (Transform child in roomParent.transform)
                {
                    child.gameObject.SetActive(false);
                }

                //generate maze
                mazeInstance = Instantiate(maze);
                var generator = mazeInstance.GetComponent<MazeGenerator>();


                if (generator != null)
                {
                    generator.OnMazeGenerated += () =>
                    {
                        if (generator.RandomEdgePos != null)
                        {
                            //player Move
                            playerCollision.transform.position = generator.RandomEdgePos.position;
                            //camera move
                            MoveCamera(playerCollision, new Vector2(400, 400));
                        }
                        else
                        {
                            Debug.LogWarning("RandomEdgePos is still null.");
                            return;
                        }
                    };
                }
                else
                {
                    return;
                   
                }

                //entrance disabled
                gameObject.GetComponent<Collider2D>().enabled = false;

            }
            else if(mazePortal==mazePortal.exit)
            {
                foreach (Transform child in roomParent.transform)
                {
                    child.gameObject.SetActive(true);
                }

                //maze disabled
                GameObject.FindWithTag("Maze").SetActive(false);
                GameObject.FindWithTag("MazeBgr").SetActive(false);


                //player move
                Vector3 backToMap = new Vector3(5, 0, 0);
                playerCollision.transform.position = GameManager.instance.nowRoom.transform.position+ backToMap;

                //camera move
                MoveCamera(playerCollision, originalCameraAreaSize);

                //Mission Complete(지금은 안해도되긴함)
                GameManager.instance.nowRoomScript.map.GetComponent<Mission>().isEscapeMaze = true;

            }
            
        }
    }

    private void MoveCamera(Collider2D target, Vector2 cameraSize)
    {
        if (target == null) return;
        CameraManager.instance.CenterMove(target.gameObject);
        CameraManager.instance.CameraMove(target.gameObject);
        CameraManager.instance.mapSize = cameraSize;
    }
}
