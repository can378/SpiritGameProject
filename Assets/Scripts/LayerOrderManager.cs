using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrderManager : MonoBehaviour
{
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
        //������ y���� �ݺ�� �Ǿ�� �ϴϱ� -1�� ���Ѵ�.
    }


}
