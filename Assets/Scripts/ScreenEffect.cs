using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum screenEffect
{ shake,fade,rumbling };

public class ScreenEffect : MonoBehaviour
{
    public bool isEffect;
    public screenEffect sEffect;
    public float shakeAmount;
    public float shakeTime;

    void Start()
    {
        if (isEffect) 
        {
            StartCoroutine("Shaking");
        }
        
    }


    //코루틴으로 바꾸기
    void Update()
    {
        
    }

    IEnumerator Shaking() 
    {
        yield return null;
        if (shakeTime > 0)
        {
            transform.position += Random.insideUnitSphere * shakeAmount;
            shakeTime -= Time.deltaTime;
        }
        else { shakeTime = 0.0f; }

        StartCoroutine("Shaking");
    }

    IEnumerator FadeInOut() 
    { 
        yield return null;
        //투명도 조절해서 만들기

    
    }
    
}
