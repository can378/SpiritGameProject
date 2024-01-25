using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // ������ ������ item���� player��ġ�� �ѱ�
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
            GetComponentInParent<Item>().SetPlayerPos(other.transform);
    }
}
