using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum SE_TYPE { NONE = -1, Slow = 0, Burn = 3, Poison = 4, Bleeding = 8, Curse = 9, END }
public enum COMMON_TYPE {NONE = -1, Thunderbolt, END}
public enum PROJECTILE_TYPE {NONE = -1, Explosion, END }

public class Enchant : MonoBehaviour
{
    // 기본 공격 관련 특수 효과 스크립트
    // 우선은 플레이어 전용 스크립트
    // 인챈트는 반드시 한개만 존재
    [field: SerializeField] public SE_TYPE SEType { get; private set; }

    [field: SerializeField] public COMMON_TYPE CommonType { get; private set; }

    [field: SerializeField] public PROJECTILE_TYPE ProjectileType { get; private set; }

    [field: SerializeField] HitDetection hitDetection;

    void Awake() 
    {
        hitDetection = GetComponent<HitDetection>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            if (CommonType == COMMON_TYPE.Thunderbolt)
                Thunderbolt(other);
        }
        
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        if (ProjectileType == PROJECTILE_TYPE.Explosion)
            Explosion();
    }

    #region Effect

    void Thunderbolt(Collider2D other)
    {

        Vector2 oCenter = other.bounds.center;
        Vector2 hCenter = hitDetection.GetComponent<Collider2D>().bounds.center;
        Vector2 dirVec = (hCenter - oCenter).normalized;
        Vector2 pos = oCenter + dirVec * 0.25f;

        Debug.Log("Crack");
        GameObject thunderboltGO = ObjectPoolManager.instance.Get("Lightning Explosion");
        thunderboltGO.transform.position = pos;
        thunderboltGO.transform.rotation = this.gameObject.transform.rotation;
        thunderboltGO.GetComponent<HitDetection>().SetHit_Ratio(5, 0.1f, hitDetection.user.GetComponent<Player>().playerStats.SkillPower);
    }

    void Explosion()
    {
        if(hitDetection.EnableType == ENABLE_TYPE.Projectile)
            return;

        Debug.Log("Bomb");
        GameObject explosionGO = ObjectPoolManager.instance.Get("Explosion");
        explosionGO.transform.position = this.gameObject.transform.position;
        explosionGO.transform.rotation = this.gameObject.transform.rotation;
        explosionGO.GetComponent<HitDetection>().SetHit(20f,10);
    }

    #endregion Effect

    public void SetSE(SE_TYPE _Type)
    {
        SEType = _Type;
        hitDetection.SetSE((int)SEType);
        // 파티클
        {
            try
            {
                ParticleSystem.MainModule particleMain = hitDetection.GetComponentInChildren<ParticleSystem>().main;
                switch (SEType)
                {
                    case SE_TYPE.Slow:
                        particleMain.startColor = Color.cyan;
                        break;
                    case SE_TYPE.Burn:
                        particleMain.startColor = new Color(255.0f / 255.0f, 100.0f / 255.0f, 0, 1.0f);
                        break;
                    case SE_TYPE.Poison:
                        particleMain.startColor = new Color(178.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f, 1.0f);
                        break;
                    case SE_TYPE.Bleeding:
                        particleMain.startColor = new Color(128.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f, 1.0f);
                        break;
                    case SE_TYPE.Curse:
                        particleMain.startColor = Color.black;
                        break;
                    default:
                        particleMain.startColor = Color.white;
                        break;
                }
            }
            catch (NullReferenceException)
            {

            }


        }
    }

    public void SetCommon(COMMON_TYPE _Type)
    {
        CommonType = _Type;
    }

    public void SetProjectile(PROJECTILE_TYPE _Type)
    {
        ProjectileType = _Type;
        switch (ProjectileType)
        {
            case PROJECTILE_TYPE.Explosion:
                hitDetection.penetrations = 0;
                break;
            default:
                break;
        }
    }
}
