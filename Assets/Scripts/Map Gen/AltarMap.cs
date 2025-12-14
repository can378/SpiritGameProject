using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AltarMap : MonoBehaviour
{
    [SerializeField] private Image darkOverlay;
    [SerializeField] private float targetAlpha = 0.6f;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine fadeCoroutine;
    
    private void Awake()
    {
        Debug.Log("Awake - Alpha before: " + darkOverlay.color.a);

        Color c = darkOverlay.color;
        c.a = 0f;
        darkOverlay.color = c;

        Debug.Log("Awake - Alpha after: " + darkOverlay.color.a);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartFade(targetAlpha);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartFade(0f);
    }

    private void StartFade(float target)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(target));
    }

    private IEnumerator Fade(float target)
    {
        float startAlpha = darkOverlay.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, target, time / fadeDuration);
            Color c = darkOverlay.color;
            c.a = a;
            darkOverlay.color = c;
            yield return null;
        }

        Color final = darkOverlay.color;
        final.a = target;
        darkOverlay.color = final;
    }
}
