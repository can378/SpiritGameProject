using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotWeapon : MainWeapon
{
    [field: SerializeField] public GameObject projectile { get; private set; }
    [field: SerializeField] public float projectileSpeed { get; private set; }
    [field: SerializeField] public float projectileTime { get; private set; }
    [field: SerializeField] public float projectileSize { get; private set; }
}
