using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactable;

public class StatSelector : MonoBehaviour, Interactable
{

    CircleCollider2D m_InteractCollider;
    [field: SerializeField] public GameObject m_InfoObject {get; private set; }

    [field: SerializeField] public Player.StatID m_StatIndex {get; private set; }

    public event System.Action InteractEvent;

    public string GetInteractText()
    {
        return "»πµÊ«œ±‚";
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

    // Ω∫≈» ¿Œµ¶Ω∫ º≥¡§
    public void SetStatIndex(Player.StatID _Idx)
    {
        m_StatIndex = _Idx;
    }

    // º±≈√¡ˆ ∫Ò»∞º∫»≠
    public void DisableSelector()
    {
        m_InteractCollider.enabled = false;
        m_InfoObject.SetActive(false);
    }

    // Ω∫≈» ¡ı∞°
    void PlayerStatLevelUp()
    {
        Player.instance.StatLevelUp((Player.StatID)m_StatIndex);

    }
}
