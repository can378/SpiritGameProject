using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 공격 판정
public class HitDetection : MonoBehaviour
{
    [field: SerializeField] public GameObject user;
    
    [field: SerializeField] public bool isProjectile { get; protected set; }                    // 투사체 여부
    [field: SerializeField] public int penetrations { get; set; }                               // 관통 횟수 필요 없을 시 음수
    [field: SerializeField] public bool isMultiHit { get; protected set; }                      // 다단히트 여부
    [field: SerializeField] public int DPS { get; protected set; }                              // 초당 타격 횟수 필요 없을 시 음수

    [field: SerializeField] public float damage { get; protected set; }
    [field: SerializeField] public float knockBack { get; protected set; }
    [field: SerializeField] public float critical { get; protected set; }
    [field: SerializeField] public float criticalDamage { get; protected set; }
    [field: SerializeField] public List<Buff> statusEffect { get; protected set; }
    [field: SerializeField] public bool isShake { get; protected set; }

    Collider2D hitCollider;
    [field: SerializeField] float multiHitDurationTime = 0;

    void Awake()
    {
        hitCollider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (isMultiHit)
        {
            MultiHit();
        }
    }

    public void SetHitDetection(
    bool isProjectile, int penetrations, bool isMultiHit, int DPS,
    float damage, float knockBack, float critical = 0.0f, float criticalDamage = 1.0f)
    {
        this.isProjectile = isProjectile;
        this.penetrations = penetrations;
        this.isMultiHit = isMultiHit;
        this.DPS = DPS;
        this.damage = damage;
        this.knockBack = knockBack;
        this.critical = critical;
        this.criticalDamage = criticalDamage;
    }

    public void SetSE(Buff statusEffect)
    {
        this.statusEffect.Clear();
        if(statusEffect == Buff.None)
            return;
        this.statusEffect.Add(statusEffect);
    }

    public void SetSEs(Buff[] statusEffect)
    {
        // 디버프 적용
        this.statusEffect.Clear();
        foreach (Buff i in statusEffect)
        {
            this.statusEffect.Add(i);
        }
    }

    public void SetShake(bool _IsShake)
    {
        isShake = _IsShake;
    }

    void MultiHit()
    {
        multiHitDurationTime += Time.deltaTime;
        if (multiHitDurationTime > (1f / DPS))
        {
            StartCoroutine(ColliderOnOff());
            multiHitDurationTime = 0;
        }
    }

    IEnumerator ColliderOnOff()
    {
        hitCollider.enabled = false;
        yield return null;
        hitCollider.enabled = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (isProjectile)
        {
            if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Door" || other.gameObject.tag == "ShabbyWall"
                 || other.CompareTag("EnemyWall"))
            {
                Destroy(gameObject);
            }

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

        if (!user)
            return;

        if (!other.gameObject.GetComponent<ObjectBasic>())
            return;

        if (other.gameObject.GetComponent<ObjectBasic>().status.isInvincible)
            return;

        user.GetComponent<ObjectBasic>().status.hitTarget = other.gameObject;
    }

    public virtual void SetProjectileTime(float time)
    {
        Destroy(this.gameObject, time);
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
