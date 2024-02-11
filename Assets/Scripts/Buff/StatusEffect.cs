using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    [field: SerializeField] public GameObject Icon { get; set; }
    [field: SerializeField] public GameObject target { get; set; }
    [field: SerializeField] public float duration {get; set;}

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}
