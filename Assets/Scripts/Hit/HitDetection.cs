using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 공격 판정
public class HitDetection : MonoBehaviour
{
    [field: SerializeField] public bool isProjectile { get; private set; }                  // 투사체 여부
    [field: SerializeField] public int penetrations { get; private set; }                   // 관통 횟수 필요 없을 시 음수
    [field: SerializeField] public bool isMultiHit { get; private set; }                    // 다단히트 여부
    [field: SerializeField] public int DPS { get; private set; }                            // 초당 타격 횟수 필요 없을 시 음수

    [field: SerializeField] public List<int> attackAttributes { get; private set; }              // 0 : 무속성, 1 : 참격, 2 : 타격, 3 : 관통, 4 : 화염, 5 : 냉기, 6 : 전기, 7 : 역장, 8 : 신성, 9 : 어둠
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public float critical { get; private set; }
    [field: SerializeField] public float criticalDamage { get; private set; }
    [field: SerializeField] public GameObject deBuff {get; private set; }

    [field: SerializeField] public List<EnemyBasic> enemies { get; private set; }

    public void SetHitDetection(
        bool isProjectile, int penetrations, bool isMultiHit, int DPS, List<int> attackAttributes,
        float damage, float knockBack, float critical, float criticalDamage, GameObject deBuff
    )
    {
        this.isProjectile = isProjectile;
        this.penetrations = penetrations;
        this.isMultiHit = isMultiHit;
        this.DPS = DPS;
        this.attackAttributes = attackAttributes;
        this.damage = damage;
        this.knockBack = knockBack;
        this.critical = critical;
        this.criticalDamage = criticalDamage;
        this.deBuff = deBuff;

        if(isMultiHit)
            StartCoroutine(MultiHit());
    }

    IEnumerator MultiHit()
    {
        while (true)
        {
            foreach (EnemyBasic enemy in enemies)
            {
                enemy.Damaged(damage, critical, criticalDamage, attackAttributes);
                if(deBuff != null) enemy.ApplyBuff(deBuff);
            }
            yield return new WaitForSeconds(1 / (float)DPS);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(isProjectile)
        {
            if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Door" || other.gameObject.tag == "ShabbyWall")
            {
                Destroy(gameObject);
            }
            else if (other.gameObject.tag == "Enemy")
            {
                if (!this.isMultiHit)
                {
                    if (this.penetrations >= 1)
                    {
                        penetrations--;
                    }
                    else if (this.penetrations == 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        if(isMultiHit)
        {
            if(other.gameObject.tag == "Enemy")
            {
                enemies.Add(other.gameObject.GetComponent<EnemyBasic>());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        
        if (other.gameObject.tag == "Enemy")
        {
            if(enemies.Contains(other.gameObject.GetComponent<EnemyBasic>()))
            {
                enemies.Remove(other.gameObject.GetComponent<EnemyBasic>());
            }
            
        }
        
    }

    
}
