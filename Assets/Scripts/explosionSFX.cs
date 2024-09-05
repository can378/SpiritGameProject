using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionSFX : MonoBehaviour
{
    private float maintainTime;
    private float time;
    void Start()
    {
        maintainTime = 2f;
        time = maintainTime;
    }
    private void OnEnable()
    {
        time = maintainTime;
    }


    void Update()
    {
        if (time > 0) { time -= 0.1f; }
        else { gameObject.SetActive(false); }
    }
}
