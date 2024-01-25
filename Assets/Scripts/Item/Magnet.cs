using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // 범위에 닿으면 item에게 player위치를 넘김
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
            GetComponentInParent<Item>().SetPlayerPos(other.transform);
    }
}
