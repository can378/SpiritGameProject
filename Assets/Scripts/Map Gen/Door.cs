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
    public void DestroyShabbyWall()
    {
        this.enabled = false;
    }

    //player 상호작용
    public void DoorInteraction()
    {
        // 키가 있다면
        if(this.doorType == DoorType.Key)
        {
            Debug.Log("열쇠를 사용하였다.");
            UnLockDoor();
        }
        // 상호작용으로는 열리지 않음
        else if (this.doorType == DoorType.Trap)
        {
            Debug.Log("열리지 않는다.");
        }
        // 폭탄을 사용하였다면
        else if(this.doorType == DoorType.ShabbyWall)
        {
            Destroy(this.gameObject);
            Debug.Log("벽이 부서졌다.");
        }
    }

    // 문 잠그기
    // none, wall, shabbywall은 조작 불가
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

    // 문 열기
    // none, wall, shabbywall은 조작 불가
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
    // wall, shabbywall은 조작 불가
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

    // 벽 설정하기
    // none, key, trap은 조작 불가
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

    // 통로와 맞닿은 wall은 shabby로 변경
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ShabbyCheck" && doorType == DoorType.Wall)
        {
            SetWallType(DoorType.ShabbyWall);
        }
    }
}
