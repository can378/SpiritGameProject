using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� ��ũ��Ʈ�� ���� ������Ʈ�� Ÿ���̴�.
public class ATarget : MonoBehaviour
{
    public void Update()
    {
        GameObject.Find("Astar").GetComponent<PathFinding>().fugitivePos 
            = (Vector2)transform.position;
    }
}
