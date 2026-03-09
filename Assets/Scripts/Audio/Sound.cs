using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    void Update()
    {
        var source = GetComponent<AudioSource>();

        if (source.clip != null && source.isPlaying == false)
        {
            AudioManager.ReturnObject(this);
        }
    }
}