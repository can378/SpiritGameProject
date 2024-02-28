using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [field: SerializeField] public float explosionTime { get; private set; }
    [field: SerializeField] public GameObject explosionField { get; private set; }

    void OnDestroy()
    {
        explosion();
    }

    void explosion()
    {
        Destroy(Instantiate(explosionField, gameObject.transform.position, gameObject.transform.rotation), explosionTime);
    }
}
