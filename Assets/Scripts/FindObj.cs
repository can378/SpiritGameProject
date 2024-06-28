using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObj : MonoBehaviour
{
    public static FindObj instance = null;
    public GameObject Player;
    public Player playerScript;
    public GameObject roomManager;
    public RoomManager roomManagerScript;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    

}
