using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform playerTransform;

    void FixedUpdate()
    {
        //player의 위치를 얻으면 player에게 다가감
        if(playerTransform == null)
        {
            return;
        }
        transform.position = Vector2.Lerp(transform.position, playerTransform.position, 0.1f);
    }
}
