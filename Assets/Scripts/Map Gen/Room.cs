using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType { Default, Lock, CrossRoad }

public class Room : MonoBehaviour
{
    private RoomManager roomManager;
    public MapType type;

    public bool top;
    public bool bottom;
    public bool left;
    public bool right;

    void Start() {
        roomManager = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomManager>();
        roomManager.room.Add(this.gameObject);
    }

}
