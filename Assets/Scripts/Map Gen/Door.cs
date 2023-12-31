using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { None, Key, Trap, Shabby, Wall }

public class Door : MonoBehaviour
{
    DoorType doorType;
    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Shabby �ı�
    public void DestroyDoor()
    {
        this.enabled = false;
    }

    //player ��ȣ�ۿ�
    public void DoorInteraction()
    {
        if(this.doorType == DoorType.Key)
        {
            Debug.Log("���踦 ����Ͽ���.");
            UnLockDoor();
        }
        else if (this.doorType == DoorType.Trap)
        {
            Debug.Log("������ �ʴ´�.");
        }
        else if(this.doorType == DoorType.Shabby)
        {
            Destroy(this.gameObject);
            Debug.Log("���� �μ�����.");
        }
    }

    // �ý��ۿ��� �� ��ױ�
    // ���� ���� �Ұ�
    public void LockDoor()
    {
        if(this.doorType == DoorType.None)
            return;
        if(this.doorType == DoorType.Shabby)
            return;
        if (this.doorType == DoorType.Wall)
            return;

        this.gameObject.SetActive(true);
    }

    // �ý��ۿ��� �� ����
    // ���� ���� �Ұ�
    public void UnLockDoor()
    {
        if (this.doorType == DoorType.None)
            return;
        if (this.doorType == DoorType.Shabby)
            return;
        if (this.doorType == DoorType.Wall)
            return;

        this.gameObject.SetActive(false);
    }

    // �� �����ϱ�
    public void SetDoorType(DoorType doorType)
    {
        this.doorType = doorType;

        // ��������Ʈ ����
        this.gameObject.SetActive(true);

        if (this.doorType == DoorType.Key)
        {
            sprite.color = Color.green;
        }
        else if (this.doorType == DoorType.Trap)
        {
            sprite.color = Color.gray;
        }

        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ShabbyCheck")
        {
            this.doorType = DoorType.Shabby;
            sprite.color = Color.cyan;
        }
    }
}
