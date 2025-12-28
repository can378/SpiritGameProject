using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AltarMap : MonoBehaviour
{
    [SerializeField] private GameObject darkEffectObject; 
    [SerializeField] private Image darkOverlay;
    [SerializeField] private float targetAlpha = 0.6f;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine fadeCoroutine;
    
    private void Awake()
    {
        if (darkOverlay != null)
        {
            Color c = darkOverlay.color;
            c.a = 0f;
            darkOverlay.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 코루틴 시작 전 오브젝트를 먼저 활성화
            if (darkEffectObject != null) darkEffectObject.SetActive(true);
            StartFade(targetAlpha);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartFade(0f);
    }

    private void StartFade(float target)
    {
        //현재 스크립트가 붙은 오브젝트가 활성화되어 있을 때만 코루틴 실행
        if (!gameObject.activeInHierarchy) return;

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

        // 만약 알파값이 0이 되었다면 오브젝트를 꺼서 최적화
        if (target <= 0f)
        {
            if (darkEffectObject != null) darkEffectObject.SetActive(false);
        }
    }

    //오브젝트가 파괴되거나 꺼질 때 코루틴 강제 종료 (에러 방지)
    private void OnDisable()
    {
        fadeCoroutine = null;
    }
}