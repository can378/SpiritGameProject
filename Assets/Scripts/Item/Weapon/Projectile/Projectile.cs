using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType {Bullet, Bomb, Arrow, Rotor}

public class Projectile : MonoBehaviour
{
    [field: SerializeField] public WeaponAttribute weaponAttribute { get; private set; }    //Bullet, Arrow, Rotor
    [field: SerializeField] public float damage { get; private set; }                       //Bullet, Arrow, Rotor
    [field: SerializeField] public float speed { get; private set; }                        //Bullet, Arrow, Rotor
    [field: SerializeField] public float time { get; private set; }                         //Bullet, Arrow, Rotor
    [field: SerializeField] public float size { get; private set; }                         //Bullet, Arrow, Rotor

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="speed"></param>
    /// <param name="time"></param>
    /// <param name="size"></param>
    /// <param name="weaponAttribute"></param>
    public void SetProjectile( float damage, float speed, float size, WeaponAttribute weaponAttribute)
    {
        this.speed = speed;
        this.damage = damage;
        this.time = time;
        this.size = size;
        this.weaponAttribute = weaponAttribute;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Door" || other.gameObject.tag == "ShabbyWall")
        {
            Destroy(gameObject);
        }
    }

}
