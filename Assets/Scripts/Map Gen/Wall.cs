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

    //player ��ȣ�ۿ�
    public void WallInteraction()
    {
        // ��ȣ�ۿ����δ� ������ ����
        if (this.wallType == WallType.Wall)
        {
            Debug.Log("�ܴ��� ���̴�.");
        }
        // ��ź�� ����Ͽ��ٸ�
        else if (this.wallType == WallType.ShabbyWall)
        {
            Debug.Log("�㸧�� ���̴�.");
        }
    }

    // �� �����ϱ�
    // none, key, trap�� ���� �Ұ�
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

    // ��ο� �´��� wall�� shabby�� ����
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
        if(hitDetection.damage >= 100)
        {
            Destroy(this.gameObject);
        }
    }
}
