using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHitDetection : HitDetection
{
    [field: SerializeField] public float size { get; private set; }          // 초당 타격수
    [field: SerializeField] public int DPS { get; private set; }          // 초당 타격수

    public void SetMultiHitDetection(int attackAttribute, float damage, float knockBack, float critical, float criticalDamage, float size, int DPS)
    {
        SetHitDetection(attackAttribute, damage, knockBack, critical, criticalDamage);
        this.size = size;
        this.DPS = DPS;
        transform.localScale = new Vector3(size, size, 0);
        StartCoroutine(MultiHit());
    }

    IEnumerator MultiHit()
    {
        RaycastHit2D[] rayHits;

        while (true)
        {
            rayHits = Physics2D.CircleCastAll(transform.position, size / 2, Vector2.up);
            foreach (RaycastHit2D hitObj in rayHits)
            {
                if (hitObj.transform.tag == "Enemy")
                    hitObj.transform.GetComponent<EnemyBasic>().Damaged(damage, critical, criticalDamage);
            }
            yield return new WaitForSeconds(1/(float)DPS);
        }

    }
}
