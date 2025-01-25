using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public enum BuffType { ABILITY , CONTINUOUS_DAMAGE , DISTURBING_EFFECT ,OVERLAP, SPECIAL, Buff }

public abstract class StatusEffect : MonoBehaviour
{
    [field: SerializeField] public string buffName { get; private set; }
    [field: SerializeField] public Image icon { get; private set; }
    [field: SerializeField] public GameObject target { get; private set; }
    [field: SerializeField] public int buffId { get; private set; }
    [field: SerializeField] public BuffType buffType { get; private set; }
    [field: SerializeField] public float defaultDuration { get; private set; }          // 기본 시간
    [field: SerializeField] public float duration {get; set; }                          // 시간
    [field: SerializeField] public int DefaultMaxOverlap { get; protected set; }               // 최대 중첩
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

    public abstract void Apply();     //추가
    public abstract void Overlap();     //갱신
    public abstract void Progress();
    public abstract void Remove();    //제거
}
