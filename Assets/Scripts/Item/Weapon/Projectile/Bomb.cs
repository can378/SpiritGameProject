using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{
    [field: SerializeField] public GameObject explosionField { get; private set; }
    
    private void OnDestroy() {
        explosion();
    }

    void explosion()
    {
        Instantiate(explosionField, gameObject.transform.position, gameObject.transform.rotation);
    }
}
