using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    Transform playerTransform;

    // ������ ������ item���� player��ġ�� �ѱ�
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
            GetComponentInParent<Item>().playerTransform = other.gameObject.transform;
    }
}
