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

    // Shabby 파괴
    public void DestroyDoor()
    {
        this.enabled = false;
    }

    //player 상호작용
    public void DoorInteraction()
    {
        if(this.doorType == DoorType.Key)
        {
            Debug.Log("열쇠를 사용하였다.");
            UnLockDoor();
        }
        else if (this.doorType == DoorType.Trap)
        {
            Debug.Log("열리지 않는다.");
        }
        else if(this.doorType == DoorType.Shabby)
        {
            Destroy(this.gameObject);
            Debug.Log("벽이 부서졌다.");
        }
    }

    // 시스템에서 문 잠그기
    // 벽은 설정 불가
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

    // 시스템에서 문 열기
    // 벽은 조작 불가
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

    // 문 설정하기
    public void SetDoorType(DoorType doorType)
    {
        this.doorType = doorType;

        // 스프라이트 변경
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
