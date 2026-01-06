using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BuffState { None, Apply, Remove}
public enum BuffType { ABILITY , CONTINUOUS_DAMAGE , DISTURBING_EFFECT ,OVERLAP, SPECIAL, Buff }

[System.Serializable]
public class Buff
{
    [HideInInspector] public BuffState m_BuffState { get; private set; } = BuffState.None;      // 버프 현재 상태 None, Apply : 대상에게 적용 중, Remove, 대상에게서 제거됨
    [field: SerializeField] public BuffData buffData { get; private set; }
    [field: SerializeField] public ObjectBasic target { get; private set; }
    [field: SerializeField] public float duration { get; set; }                              // 버프 지속 시간
    [field: SerializeField] public float curDuration { get; set; }                          // 시간
    [field: SerializeField] public int stack { get; set; }                      // 현재 중첩
    [field: SerializeField] public TMP_Text overlapText { get; private set; }

    [field: SerializeField] public Dictionary<string, object> CustomData { get; private set; }

    public Buff(BuffData _BuffData, ObjectBasic _Target)
    {
        buffData = _BuffData;
        target = _Target;
        CustomData = new Dictionary<string, object>();
    }

    void Update()
    {
        buffData.icon.fillAmount = curDuration / duration;
        //if (this.target == null)
        //    Destroy(this.gameObject);
    }

    public void AddStack()
    {
        stack = stack < buffData.DefaultMaxStack ? stack + 1 : buffData.DefaultMaxStack;
        //overlapText.text = stack > 1 ? stack.ToString() : null;

    }

    public void Apply()     //추가
    {
        buffData.Apply(this);
        m_BuffState = BuffState.Apply;
    }
    public void Overlap()     //갱신
    {
        buffData.Overlap(this);
    }
    public void Update_Buff()
    {
        buffData.Update_Buff(this);
    }
    public void Remove()    //제거
    {
        buffData.Remove(this);
        m_BuffState = BuffState.Remove;
    }

}
