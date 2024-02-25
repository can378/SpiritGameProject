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
        // 0 : ���Ӽ�, 1 : ����, 2 : Ÿ��, 3 : ����, 4 : ȭ��, 5 : �ñ�, 6 : ����, 7 : ����, 8 : �ż�, 9 : ���
        if ((hitDetection.attackAttribute == 2 || hitDetection.attackAttribute == 7))
        {
            if(hitDetection.damage < 20)
            {
                Debug.Log("�ʹ� ���ϴ�.");
            }
            else
            {
                Debug.Log("���� ��������.");
                Destroy(this.gameObject);
            }
        }
    }
}
