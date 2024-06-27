using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum screenEffect
{ none, shake, fadeIn, fadeOut, rumbling, bleeding, rumbling1, rumbling2 };

public class ScreenEffect : MonoBehaviour
{
    public screenEffect sEffect;
    public float shakeAmount;
    public float shakeTime;
    public Image fade;
    public GameObject fadePanel;
    public Image edgeEffect;
    public GameObject effectPanel;


    void Start()
    {
        switch (sEffect)
        {
            case screenEffect.shake:
                StartCoroutine("Shaking");
                break;
            case screenEffect.fadeIn:
                StartCoroutine("FadeIn");
                break;
            case screenEffect.fadeOut:
                StartCoroutine("FadeOut");
                break;
            case screenEffect.rumbling:
                StartCoroutine("Rumbling");
                break;
            case screenEffect.bleeding:
                StartCoroutine(Bleeding());
                break;
            case screenEffect.rumbling1:
                break;
            case screenEffect.rumbling2:
                break;
            default:break;
        }


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

    IEnumerator FadeIn()
    {
        float fadeCount = 0;


        fadePanel.SetActive(true);
        while (fadeCount <= 1)
        {
            fade.color = new Color(0, 0, 0, fadeCount);
            fadeCount += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        fade.color = new Color(0, 0, 0, 1);
        fadePanel.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        float fadeCount = 1;
        fadePanel.SetActive(true);
        while (fadeCount >= 0)
        {
            fade.color = new Color(0, 0, 0, fadeCount);
            fadeCount -= 0.1f;

            yield return new WaitForSeconds(0.1f);
        }
        fade.color = new Color(0, 0, 0, 0);
        fadePanel.SetActive(false);
    }
    IEnumerator Rumbling()
    {
        yield return new WaitForSeconds(0.1f);


    }

    IEnumerator Bleeding()
    {
        effectPanel.SetActive(true);
        edgeEffect.color *= new Color(1, 1, 1, 0f);


        //clear
        while (edgeEffect.color.a < 0.3)
        {
            edgeEffect.color += new Color(0, 0, 0, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        //blurry
        while (edgeEffect.color.a > 0)
        {
            edgeEffect.color += new Color(0, 0, 0, -0.1f);
            yield return new WaitForSeconds(0.1f); 
        }
        
        
        StartCoroutine(Bleeding());
    }



    public void stopBleeding() 
    {
        StopCoroutine(Bleeding());
        effectPanel.SetActive(false);
    }


}
