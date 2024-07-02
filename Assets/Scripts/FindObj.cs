using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObj : MonoBehaviour
{
    //이걸 gameManger에 합칠지 아니면 gamemangetr죽ㅇ에 일부를 여기로 할디 고민중
    public static FindObj instance = null;
    public GameObject Player;
    public Player playerScript;
    public GameObject roomManager;
    public RoomManager roomManagerScript;
    public GameObject roomParent;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    

}
