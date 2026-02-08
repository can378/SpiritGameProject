using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactable;
using TMPro;

public class StatSelector : MonoBehaviour, Interactable
{

    CircleCollider2D m_InteractCollider;
    [field: SerializeField] public GameObject m_InfoObject {get; private set; }
    [field: SerializeField] public SpriteRenderer m_Icon {get; private set; }
    [field: SerializeField] public TMP_Text m_StatName { get; private set; }

    [field: SerializeField] public List<SpriteRenderer> m_DisableRenderer { get; private set; }

    [field: SerializeField] public SOStatData m_StatData {get; private set; }

    public event System.Action InteractEvent;

    public string GetInteractText()
    {
        return m_StatData.m_StatName +" " + m_StatData.m_ActionDescription.m_Description;
    }

    public void Interact()
    {
        PlayerStatLevelUp();
        InteractEvent?.Invoke();
    }

    void Awake()
    {
        m_InteractCollider = GetComponent<CircleCollider2D>();
    }

    // 스탯 인덱스 설정
    public void SetStatIndex(SOStatData _StatData)
    {
        m_StatData = _StatData;
        m_Icon.sprite = m_StatData.m_Icon;
        m_StatName.text = m_StatData.m_StatName;
    }

    // 선택지 비활성화
    public void DisableSelector()
    {
        m_InteractCollider.enabled = false;
        m_StatName.gameObject.SetActive(false);
        //m_InfoObject.SetActive(false);
        StartCoroutine(DiableEffect());
    }

    IEnumerator DiableEffect()
    {
        float time = 1;
        while(0.0f < time)
        {
            time -= Time.deltaTime;
            foreach(SpriteRenderer renderer in m_DisableRenderer)
            {
                Color color = renderer.color;
                color.a = time;
                renderer.color = color;
            }
            yield return null;
        }

        yield return null;
    }

    // 스탯 증가
    void PlayerStatLevelUp()
    {
        Player.instance.StatLevelUp(m_StatData.m_StatID);

    }
}
