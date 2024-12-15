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
    }

    void Explosion()
    {
        if(!hitDetection.isProjectile)
            return;

        Debug.Log("Bomb");
        GameObject explosionGO = ObjectPoolManager.instance.Get2("Explosion");
        explosionGO.transform.position = this.gameObject.transform.position;
        explosionGO.transform.rotation = this.gameObject.transform.rotation;
        explosionGO.GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, 20f, 0);
    }

    #endregion Effect


}
