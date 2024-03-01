using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffField : MonoBehaviour
{
    public GameObject Buff;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.ApplyBuff(Buff);
        }
        else if(other.tag == "Enemy")
        {
            EnemyBasic enemy = other.GetComponent<EnemyBasic>();
            enemy.ApplyBuff(Buff);
        }
    }
}
