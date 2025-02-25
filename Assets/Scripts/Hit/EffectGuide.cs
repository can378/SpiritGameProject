using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ÿ�� ������ �����Ǳ� �� ����Ʈ ���̵� ��ũ��Ʈ
// ������ ����, �÷��̾��� ȭ���� ��ų ��
// ���̵� �ð��� ������ �ݶ��̴��� �ߵ�
public class EffectGuide : MonoBehaviour
{
    [Header("���̵� �ð��� ������ ���� ���� ����")]
    [SerializeField]
    float m_GuideTime;      // ����Ʈ ���̵� �ð�
    SpriteRenderer m_GuideSprite;   // ���̵� ��������Ʈ
    [SerializeField]
    [Header("���� ����")]
    GameObject m_Effect;      // Ÿ�� ���� ������Ʈ
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

    // ���̵� Renderer�� ���� ���� ������ Ų��.
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
