using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemClass { Coin, Key, BossKey, Heal, MaxHealth, ExtraHealth, EXP};

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemClass itemClass {get; private set;}

    Transform playerTransform;

    void FixedUpdate()
    {
        //player�� ��ġ�� ������ player���� �ٰ���
        if(playerTransform == null)
        {
            return;
        }
        transform.position = Vector2.Lerp(transform.position, playerTransform.position, 0.1f);
    }

    public void SetPlayerPos(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
}
