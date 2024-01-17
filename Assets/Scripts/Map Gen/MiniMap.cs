using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject miniMapCamera;
    void Start()
    {
        
    }

    void setMinimapCamera()
    {
        Transform roomParent = GameObject.FindWithTag("roomParent").transform;
        Transform eachRoom;
        float xMin, xMax, yMin, yMax;

        xMin = roomParent.GetChild(0).position.x;
        xMax = roomParent.GetChild(0).position.x;
        yMin = roomParent.GetChild(0).position.y;
        yMax = roomParent.GetChild(0).position.y;

        for (int i = 1; i < roomParent.childCount; i++)
        {
            eachRoom = roomParent.GetChild(i);
            if (eachRoom.position.x < xMin) { xMin = eachRoom.position.x; }
            if (eachRoom.position.x > xMax) { xMax = eachRoom.position.x; }
            if (eachRoom.position.y < yMin) { yMin = eachRoom.position.y; }
            if (eachRoom.position.y > yMax) { yMax = eachRoom.position.y; }
        }

        print(xMin + " " + xMax + " " + yMin + " " + yMax);

    }
}
