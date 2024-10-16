using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    enum DirectionType {TOP,BOTTOM,LEFT,RIGHT, NONE}
    
    [field : SerializeField]
    DirectionType type;

    [field : SerializeField]
    Room room;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Wall")
        {
            switch(type)
            {
                case DirectionType.TOP:
                {
                    room.top = false;
                }
                break;
                case DirectionType.BOTTOM:
                {
                    room.bottom = false;
                }
                break;
                case DirectionType.LEFT:
                {
                    room.left = false;
                }
                break;
                case DirectionType.RIGHT:
                {
                    room.right = false;
                }
                break;
                case DirectionType.NONE:
                break;
            }
        }
    }
}
