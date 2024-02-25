using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallType { Wall, ShabbyWall }

public class Wall : MonoBehaviour
{
    [SerializeField]
    WallType wallType;
    SpriteRenderer sprite;

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
            Collapse(other.GetComponent<HitDetection>());
        }
    }

    public void Collapse(HitDetection hitDetection)
    {
        // 0 : 무속성, 1 : 참격, 2 : 타격, 3 : 관통, 4 : 화염, 5 : 냉기, 6 : 전기, 7 : 역장, 8 : 신성, 9 : 어둠
        if ((hitDetection.attackAttribute == 2 || hitDetection.attackAttribute == 7))
        {
            if(hitDetection.damage < 20)
            {
                Debug.Log("너무 약하다.");
            }
            else
            {
                Debug.Log("벽이 무너졌다.");
                Destroy(this.gameObject);
            }
        }
    }
}
