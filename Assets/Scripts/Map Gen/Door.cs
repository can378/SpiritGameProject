using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { None, Key, Trap, ShabbyWall, Wall }

public class Door : MonoBehaviour
{
    [SerializeField]
    DoorType doorType;
    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Shabby �ı�
    public void DestroyShabbyWall()
    {
        this.enabled = false;
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
        // ��ź�� ����Ͽ��ٸ�
        else if(this.doorType == DoorType.ShabbyWall)
        {
            Destroy(this.gameObject);
            Debug.Log("���� �μ�����.");
        }
    }

    // �� ��ױ�
    // none, wall, shabbywall�� ���� �Ұ�
    public void LockDoor()
    {
        if(this.doorType == DoorType.None)
            return;
        if(this.doorType == DoorType.ShabbyWall)
            return;
        if (this.doorType == DoorType.Wall)
            return;

        this.gameObject.SetActive(true);
    }

    // �� ����
    // none, wall, shabbywall�� ���� �Ұ�
    public void UnLockDoor()
    {
        if (this.doorType == DoorType.None)
            return;
        if (this.doorType == DoorType.ShabbyWall)
            return;
        if (this.doorType == DoorType.Wall)
            return;

        this.gameObject.SetActive(false);
    }

    // �� �����ϱ�
    // wall, shabbywall�� ���� �Ұ�
    public void SetDoorType(DoorType doorType)
    {
        if (this.doorType == DoorType.ShabbyWall)
            return;
        if (this.doorType == DoorType.Wall)
            return;

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

    // �� �����ϱ�
    // none, key, trap�� ���� �Ұ�
    void SetWallType(DoorType doorType)
    {
        if(this.doorType == DoorType.None)
            return;
        if (this.doorType == DoorType.Key)
            return;
        if (this.doorType == DoorType.Trap)
            return;

        this.doorType = doorType;

        if (this.doorType == DoorType.ShabbyWall)
        {
            sprite.color = Color.cyan;
        }
        else if (this.doorType == DoorType.Wall)
        {
            sprite.color = Color.black;
        }
    }

    // ��ο� �´��� wall�� shabby�� ����
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ShabbyCheck" && doorType == DoorType.Wall)
        {
            SetWallType(DoorType.ShabbyWall);
        }
    }
}
