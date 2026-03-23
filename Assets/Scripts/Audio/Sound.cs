using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (source.clip != null && !source.isPlaying)
        {
            AudioManager.ReturnObject(this);
        }
    }
}