using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { None, Key, Trap }

// 문 상태
/// <summary>
/// Open : 문이 열려있는 상태, 문을 잠글 수 있음
/// Lock : 문이 잠겨있는 상태, 문이 UnLock 상태가 될 수 있음
/// UnLock : 문이 잠금 해제된 상태, 다시 잠기지 않음
/// </summary>
public enum DoorState { Open, Lock, UnLock }

public class Door : MonoBehaviour, Interactable
{
    [SerializeField]
    DoorType doorType;

    [SerializeField]
    DoorState doorState;
    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public string GetInteractText()
    {
        return "문 열기";
    }

    //player 상호작용
    public void Interact()
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
        // 기본 상태라면 반환
        // 열려있는 상태가 아니라면 반환
        if (this.doorType == DoorType.None || this.doorState != DoorState.Open)
            return;
        doorState = DoorState.Lock;
        this.gameObject.SetActive(true);
    }

    // 문 열기
    // none, wall, shabbywall은 조작 불가
    public void UnLockDoor()
    {
        doorState = DoorState.UnLock;
        this.gameObject.SetActive(false);
    }

    // 문 설정하기
    // wall, shabbywall은 조작 불가
    public void SetDoorType(DoorType doorType)
    {
        this.doorType = doorType;
    }

    // 문이 열려있는지 확인
    public bool IsClosed()
    {
        return this.gameObject.activeSelf;
    }
}
