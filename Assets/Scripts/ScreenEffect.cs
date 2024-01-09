using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum screenEffect
{ none, shake, fadeIn, fadeOut, rumbling };

public class ScreenEffect : MonoBehaviour
{
    public screenEffect sEffect;
    public float shakeAmount;
    public float shakeTime;
    public Image fade;
    public GameObject fadePanel;

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


}
