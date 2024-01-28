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
    }

    // 문 잠그기
    // none, wall, shabbywall은 조작 불가
    public void LockDoor()
    {
        if(this.doorType == DoorType.None)
            return;

        this.gameObject.SetActive(true);
    }

    // 문 열기
    // none, wall, shabbywall은 조작 불가
    public void UnLockDoor()
    {
        this.gameObject.SetActive(false);
    }

    // 문 설정하기
    // wall, shabbywall은 조작 불가
    public void SetDoorType(DoorType doorType)
    {
        this.doorType = doorType;
    }
}
