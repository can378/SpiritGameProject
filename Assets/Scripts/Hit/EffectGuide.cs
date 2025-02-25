using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타격 판정이 생성되기 전 이펙트 가이드 스크립트
// 저퀴의 눈물, 플레이어의 화염구 스킬 등
// 가이드 시간이 끝나면 콜라이더가 발동
public class EffectGuide : MonoBehaviour
{
    [Header("가이드 시간이 끝나면 공격 판정 생성")]
    [SerializeField]
    float m_GuideTime;      // 이펙트 가이드 시간
    SpriteRenderer m_GuideSprite;   // 가이드 스프라이트
    [SerializeField]
    [Header("직접 설정")]
    GameObject m_Effect;      // 타격 판정 오브젝트
    Collider2D m_Collider;
    HitDetection m_HD;

    void Awake()
    {
        m_GuideSprite = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<Collider2D>();
        m_HD = GetComponent<HitDetection>();
    }

    void OnEnable()
    {
        StartCoroutine("Guide");
    }

    // 가이드 Renderer를 끄고 공격 판정을 킨다.
    IEnumerator Guide()
    {
        m_Effect.SetActive(false);
        m_GuideSprite.enabled = true;
        m_Collider.enabled = false;
        m_HD.enabled = false;

        yield return new WaitForSeconds(m_GuideTime);

        m_HD.enabled = true;
        m_Collider.enabled = true;
        m_GuideSprite.enabled = false;
        m_Effect.SetActive(true);
    }


}
