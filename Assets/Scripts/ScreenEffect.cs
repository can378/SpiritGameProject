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


    //�ڷ�ƾ���� �ٲٱ�
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
        //���� �����ؼ� �����

    
    }
    
}
