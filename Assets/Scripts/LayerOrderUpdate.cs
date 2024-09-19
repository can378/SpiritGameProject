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
        //포지션 y값에 반비례 되어야 하니까 -1를 곱한다.
    }
}
