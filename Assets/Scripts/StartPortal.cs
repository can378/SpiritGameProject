using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPortal : Portal
{
    RoomManager roomManager;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
            Destination = roomManager.rooms[0].transform;
            other.transform.position = Destination.position;
        }
    }
}
