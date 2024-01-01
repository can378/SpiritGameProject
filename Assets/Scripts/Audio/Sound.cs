using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{

    void Update()
    {
        if (this.GetComponent<AudioSource>().isPlaying == false)
        { AudioManager.ReturnObject(this); }
    }
}