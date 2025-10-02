using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffField : MonoBehaviour
{
    public BuffData[] Buff;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            for(int i = 0 ; i<Buff.Length;i++)
            {
                player.ApplyBuff(Buff[i]);
            }
            
        }
        else if(other.tag == "Enemy")
        {
            EnemyBasic enemy = other.GetComponent<EnemyBasic>();
            for (int i = 0; i < Buff.Length; i++)
            {
                enemy.ApplyBuff(Buff[i]);
            }
        }
    }
}
