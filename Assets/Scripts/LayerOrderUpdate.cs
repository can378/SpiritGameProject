using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrderUpdate : MonoBehaviour
{
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
        //������ y���� �ݺ�� �Ǿ�� �ϴϱ� -1�� ���Ѵ�.
    }
}
