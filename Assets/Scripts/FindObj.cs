using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObj : MonoBehaviour
{
    //�̰� gameManger�� ��ĥ�� �ƴϸ� gamemangetr�פ��� �Ϻθ� ����� �ҵ� �����
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
