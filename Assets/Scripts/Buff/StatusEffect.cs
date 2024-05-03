using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    [field: SerializeField] public GameObject icon { get; set; }
    [field: SerializeField] public GameObject target { get; set; }
    [field: SerializeField] public int buffId { get; set; }
    [field: SerializeField] public float defaultDuration { get; set; }          // �⺻ �ð�
    [field: SerializeField] public float duration {get; set;}               // �ð�
    [field: SerializeField] public int maxOverlap { get; set; }             // �ִ� ��ø
    [field: SerializeField] public int overlap { get; set; }                // ���� ��ø

    void Update() {
        if(this.target == null)
            Destroy(this.gameObject);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public abstract void ApplyEffect();     //�߰�
    public abstract void ResetEffect();     //����
    public abstract void RemoveEffect();    //����
}
