using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{
    [field: SerializeField] public float explosionTime { get; private set; }
    [field: SerializeField] public GameObject explosionField { get; private set; }
    
    private void OnDestroy() {
        explosion();
    }

    void explosion()
    {
        GameObject explonsionGameObject = Instantiate(explosionField, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(explonsionGameObject, explosionTime);
    }
}
