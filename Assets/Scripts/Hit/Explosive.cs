using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��ü
// ���� ����Ʈ�� �ִ´�.
public class Explosive : Projectile
{
    [field: SerializeField] public float explosionTime { get; private set; }
    [field: SerializeField] public GameObject explosionField { get; private set; }
    
    void OnDestroy() {
        explosion();
    }

    // ����
    void explosion()
    {
        GameObject explonsionGameObject = Instantiate(explosionField, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(explonsionGameObject, explosionTime);
    }

    // ����ü ����
    public void SetExplosive(WeaponAttribute weaponAttribute, float damage, float knockBack, float critical, float criticalDamage,
        float size, float time)
    {
        HitDetection hitDetection = explosionField.GetComponent<HitDetection>();
        hitDetection.SetHitDetection(weaponAttribute, damage, knockBack, critical, criticalDamage);

        // ���� ���� ����
        explosionField.transform.localScale = new Vector3(size,size,1);
        // ���� ���� �ð� ����
        explosionTime = time;
    }
}
