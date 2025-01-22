using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public enum Buff {None = -1, Slow, PowerWeaken, DefensiveWeaken, Burn, Poison, Stun, Fear, Root, Bleeding, Curse, End}

public abstract class StatusEffect : MonoBehaviour
{
    [field: SerializeField] public Image icon { get; private set; }
    [field: SerializeField] public GameObject target { get; private set; }
    [field: SerializeField] public int buffId { get; private set; }
    [field: SerializeField] public float defaultDuration { get; private set; }          // 기본 시간
    [field: SerializeField] public float duration {get; set; }                          // 시간
    [field: SerializeField] public int maxOverlap { get; protected set; }               // 최대 중첩
    [field: SerializeField] public int overlap { get; protected set; }                  // 현재 중첩

    void Update() {
        icon.fillAmount = duration / defaultDuration;
        if(this.target == null)
            Destroy(this.gameObject);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetDefaultDuration (float time)
    {
        this.defaultDuration = time;
    }

    public abstract void ApplyEffect();     //추가
    public abstract void ResetEffect();     //갱신
    public abstract void RemoveEffect();    //제거
}
