using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BuffData : ScriptableObject
{
    [field: SerializeField] public string buffName { get; private set; }
    [field: SerializeField] public Image icon { get; private set; }

    [field: SerializeField] public int buffID { get; private set; }
    [field: SerializeField] public BuffType buffType { get; private set; }
    [field: SerializeField] public float defaultDuration { get; private set; }          // 기본 시간

    [field: SerializeField] public int DefaultMaxStack { get; protected set; }               // 최대 중첩

    public abstract void Update_Buff(Buff _Buff);
    public abstract void Apply(Buff _Buff);
    public abstract void Overlap(Buff _Buff);
    public abstract void Remove(Buff _Buff);
}
