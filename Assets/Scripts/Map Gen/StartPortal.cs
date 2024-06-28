using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPortal : Portal
{
    RoomManager roomManager;
    
    private void Start()
    {
        roomManager = FindObj.instance.roomManagerScript;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && roomManager.finish==true)
        {
            MapUIManager.instance.UpdateMinimapUI(true);
            Destination = roomManager.rooms[0].transform;
            other.transform.position = Destination.position;
            CameraManager.instance.CameraMove(other.gameObject);
            CameraManager.instance.CenterMove(other.gameObject);
        }


    }

}
