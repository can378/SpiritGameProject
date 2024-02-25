using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : HitDetection
{
    [field: SerializeField] public float size { get; private set; }          // 초당 타격수

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Door" || other.gameObject.tag == "ShabbyWall")
        {
            Destroy(gameObject);
        }
    }

    public void SetProjectile(int attackAttribute, float damage, float knockBack, float critical, float criticalDamage, float size)
    {
        SetHitDetection(attackAttribute, damage,knockBack,critical,criticalDamage);
        this.size = size;
        transform.localScale = new Vector3(size,size,0);
    }

}
