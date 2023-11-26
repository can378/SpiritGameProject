using System.Collections.Generic;
using UnityEngine;

public class MinimpController : MonoBehaviour
{
    public Transform playerTransform; // ���� �÷��̾� Transform
    public float iconScale = 1.5f; // �������� ũ�� ����
    void Start()
    {
        // �������� ũ�⸦ ������ ������ ����
        transform.localScale = new Vector3(iconScale, iconScale, iconScale);
    }
    void Update()
    {
        // �������� ��ġ�� �÷��̾��� ���� ��ġ�� ������Ʈ
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}