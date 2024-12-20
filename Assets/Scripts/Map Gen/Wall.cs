using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallType { Wall, ShabbyWall }

public class Wall : MonoBehaviour
{
    [SerializeField]
    WallType wallType;
    SpriteRenderer sprite;

    int durabilityCount = 10;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    //player 상호작용
    public void WallInteraction()
    {
        // 상호작용으로는 열리지 않음
        if (this.wallType == WallType.Wall)
        {
            Debug.Log("단단한 벽이다.");
        }
        // 폭탄을 사용하였다면
        else if (this.wallType == WallType.ShabbyWall)
        {
            Debug.Log("허름한 벽이다.");
        }
    }

    // 벽 설정하기
    // none, key, trap은 조작 불가
    void SetWallType(WallType wallType)
    {
        this.wallType = wallType;
        if (this.wallType == WallType.ShabbyWall)
        {
            sprite.color = Color.cyan;
        }
        else if (this.wallType == WallType.Wall)
        {
            sprite.color = Color.black;
        }
    }

    // 통로와 맞닿은 wall은 shabby로 변경
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ShabbyCheck" && wallType == WallType.Wall)
        {
            SetWallType(WallType.ShabbyWall);
        }

        if(other.tag == "PlayerAttack" && wallType == WallType.ShabbyWall)
        {
            Collapse();
        }
    }

    public void Collapse()
    {
        durabilityCount--;
        if (durabilityCount <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
