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

    // Shabby 파괴
    public void DestroyWall()
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
        else if(this.doorType == DoorType.ShabbyWall)
        {
            Destroy(this.gameObject);
            Debug.Log("벽이 부서졌다.");
        }
    }

    // 시스템에서 문 잠그기
    // 벽과 허름한 벽, 아무것도 없는 것은 조작 불가
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

    // 시스템에서 문 열기
    // 벽과 허름한 벽, 아무것도 없는 것은 조작 불가
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

    // 문 설정하기
    // 벽과 허름한 벽은 조작 불가
    public void SetDoorType(DoorType doorType)
    {
        if (this.doorType == DoorType.ShabbyWall)
            return;
        if (this.doorType == DoorType.Wall)
            return;

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

    public void SetWallType(DoorType doorType)
    {
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

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ShabbyCheck")
        {
            SetWallType(DoorType.ShabbyWall);
        }
    }
}
