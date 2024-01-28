using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { None, Key, Trap}

public class Door : MonoBehaviour
{
    [SerializeField]
    DoorType doorType;
    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }


    //player ��ȣ�ۿ�
    public void DoorInteraction()
    {
        // Ű�� �ִٸ�
        if(this.doorType == DoorType.Key)
        {
            Debug.Log("���踦 ����Ͽ���.");
            UnLockDoor();
        }
        // ��ȣ�ۿ����δ� ������ ����
        else if (this.doorType == DoorType.Trap)
        {
            Debug.Log("������ �ʴ´�.");
        }
    }

    // �� ��ױ�
    // none, wall, shabbywall�� ���� �Ұ�
    public void LockDoor()
    {
        if(this.doorType == DoorType.None)
            return;

        this.gameObject.SetActive(true);
    }

    // �� ����
    // none, wall, shabbywall�� ���� �Ұ�
    public void UnLockDoor()
    {
        this.gameObject.SetActive(false);
    }

    // �� �����ϱ�
    // wall, shabbywall�� ���� �Ұ�
    public void SetDoorType(DoorType doorType)
    {
        this.doorType = doorType;
    }
}
