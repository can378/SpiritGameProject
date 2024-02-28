using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float[] resist = new float[10]; 
    public float maxHealth;
    public float health;
    public float speed;
    public int damage;
    public int detectionDis;
    public int attackSpeed;
}
