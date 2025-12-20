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
            if (col2D.OverlapPoint(collision.GetComponent<ObjectBasic>().CenterPivot.position))
            {
                Trap();
            }
        }
    }

    // 방에 완전히 입장
    void Trap()
    {
        if (room.doorType == DoorType.Trap && !room.door.IsClosed())
        {
            room.LockDoor();
        }
        // 방에 완전히 입장했으니 이 스크립트는 파괴
        Destroy(this);
    }
}
