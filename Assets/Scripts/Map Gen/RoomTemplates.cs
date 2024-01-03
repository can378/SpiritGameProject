using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject[] verticalCrossedRooms;
    public GameObject[] horizontalCrossedRooms;

    public GameObject closedRoom;

    // maptype
    public GameObject shopIcon;
    public GameObject treasureIcon;
    public GameObject missionIcon;
    public GameObject miniBossIcon;
    public GameObject bossIcon;
    // doortype
    public GameObject trapIcon;
    public GameObject keyIcon;
}
