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
    [field: SerializeField] public float defaultDuration { get; private set; }          // �⺻ �ð�
    [field: SerializeField] public float duration {get; set; }                          // �ð�
    [field: SerializeField] public int maxOverlap { get; protected set; }               // �ִ� ��ø
    [field: SerializeField] public int overlap { get; protected set; }                  // ���� ��ø

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

    public abstract void ApplyEffect();     //�߰�
    public abstract void ResetEffect();     //����
    public abstract void RemoveEffect();    //����
}
