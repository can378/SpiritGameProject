using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TrapEnter : MonoBehaviour
{
    public Room room;
    Collider2D col2D;

    private void Awake()
    {
        col2D = GetComponent<Collider2D>();
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //print("Trigger 충돌");
            if (col2D.OverlapPoint(collision.GetComponent<ObjectBasic>().CenterPivot.position))
            {
                //print(room.gameObject.name);
                Trap();
            }
        }
    }

    // 방에 완전히 입장
    void Trap()
    {
        if (room.doorType == DoorType.Trap)
        {
            room.LockDoor();
        }
    }
}
