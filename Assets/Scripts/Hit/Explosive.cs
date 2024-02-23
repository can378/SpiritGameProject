using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터지는 물체
// 폭발 이펙트를 넣는다.
public class Explosive : Projectile
{
    [field: SerializeField] public float explosionTime { get; private set; }
    [field: SerializeField] public GameObject explosionField { get; private set; }
    
    void OnDestroy() {
        explosion();
    }

    // 폭발
    void explosion()
    {
        GameObject explonsionGameObject = Instantiate(explosionField, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(explonsionGameObject, explosionTime);
    }

    // 폭발체 설정
    public void SetExplosive(WeaponAttribute weaponAttribute, float damage, float knockBack, float critical, float criticalDamage,
        float size, float time)
    {
        HitDetection hitDetection = explosionField.GetComponent<HitDetection>();
        hitDetection.SetHitDetection(weaponAttribute, damage, knockBack, critical, criticalDamage);

        // 폭발 범위 설정
        explosionField.transform.localScale = new Vector3(size,size,1);
        // 폭발 유지 시간 설정
        explosionTime = time;
    }
}
