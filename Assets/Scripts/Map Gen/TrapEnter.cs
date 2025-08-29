using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEnter : MonoBehaviour
{
    public Room room;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Trap();
        }
    }

    void Trap()
    {
        if (room.doorType == DoorType.Trap)
        {
            room.LockDoor();
        }
    }
}
