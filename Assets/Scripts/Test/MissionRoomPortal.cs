using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionRoomPortal : MonoBehaviour
{
    RoomManager roomManager;
    List<Transform> missionRooms; // 각 미션방 위치들
    public int roomId; // 이동할 방 인덱스
    private bool isConnected = false;
    private bool triedConnect = false;

    private void Start()
    {
        roomManager = FindObj.instance.roomManagerScript;
    }

    private void OnEnable()
    {
        isConnected = false;
        triedConnect=false;
        if (roomManager == null)
        {
            roomManager = FindObj.instance.roomManagerScript;
        }
    }

    

    private void Update()
    {
        if (!isConnected && !triedConnect && roomManager != null && roomManager.finish)
        {
            triedConnect = true; // 중복 호출 방지
            missionRooms = new List<Transform>();
            connectDoor();
        }
    }


    

    public void connectDoor()
    {
        for (int i = 0; i < roomManager.rooms.Count; i++)
        {
            Room roomScript = roomManager.rooms[i].GetComponent<Room>();
            if (roomScript != null && roomScript.mapType == MapType.Mission)
            {
                missionRooms.Add(roomManager.rooms[i].transform);
            }
        }
        isConnected = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("try to enter mission room");
        if (other.CompareTag("Player") && roomManager.finish && isConnected)
        {
            Debug.Log("ready to move to mission room");
            MapUIManager.instance.UpdateMinimapUI(true);


                // 이동
                FindObj.instance.Player.transform.position = missionRooms[roomId].position;
                CameraManager.instance.CameraMove(FindObj.instance.Player.gameObject);
                CameraManager.instance.CenterMove(FindObj.instance.Player.gameObject);
            

        }
    }
}
