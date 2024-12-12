using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENCHANT_TYPE {NONE = -1, Thunderbolt, COMMON_END, Explosion, PROJECTILE_END, END}

public class Enchant : MonoBehaviour
{
    // 기본 공격 관련 특수 효과 스크립트
    // 우선은 플레이어 전용 스크립트
    // 인챈트는 반드시 한개만 존재

    [field: SerializeField] public ENCHANT_TYPE EnchantType { get; set; } = ENCHANT_TYPE.END; 

    HitDetection hitDetection;
    /*
    0 특수 효과 없음

    1 ~ 10  상태이상 추가

    11 ~ 20 공통 특수 효과
    11 타격 성공 시 벼락

    21 ~ 30 원거리 전용 특수 효과
    21 파괴 시 폭발

    */
    void Awake() 
    {
        hitDetection = GetComponent<HitDetection>();
    }

    public void SetEnchant(ENCHANT_TYPE _TYPE)
    {
        this.EnchantType = _TYPE;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(EnchantType == ENCHANT_TYPE.Thunderbolt)
            Thunderbolt(other);
    }

    void OnDisable()
    {
        if (EnchantType == ENCHANT_TYPE.Explosion)
            Explosion();
    }

    #region Effect

    //11
    void Thunderbolt(Collider2D other)
    {

        Vector2 oCenter = other.bounds.center;
        Vector2 hCenter = hitDetection.GetComponent<Collider2D>().bounds.center;
        Vector2 dirVec = (hCenter - oCenter).normalized;
        Vector2 pos = oCenter + dirVec * 0.25f;

        Debug.Log("Crack");
        GameObject thunderboltGO = ObjectPoolManager.instance.Get2("Lightning Explosion");
        thunderboltGO.transform.position = pos;
        thunderboltGO.transform.rotation = this.gameObject.transform.rotation;
        thunderboltGO.GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, 5 + this.hitDetection.user.GetComponent<Player>().playerStats.skillPower * 0.1f, 0);
        thunderboltGO.GetComponent<HitDetection>().SetProjectileTime(0.1f);
    }

    //21
    void Explosion()
    {
        if(!hitDetection.isProjectile)
            return;

        Debug.Log("Bomb");
        AudioManager.instance.SFXPlay("Explosion");
        GameObject explosionGO = ObjectPoolManager.instance.Get2("Explosion");
        explosionGO.transform.position = this.gameObject.transform.position;
        explosionGO.transform.rotation = this.gameObject.transform.rotation;
        explosionGO.GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, 20f, 0);
        explosionGO.GetComponent<HitDetection>().SetProjectileTime(0.3f);
    }

    #endregion Effect


}
