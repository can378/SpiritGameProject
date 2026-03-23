using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : EnemyBasic
{
    [SerializeField]
    GameObject m_Pus;       // Á×À» ¶§ Æø¹ßÇÏ´Â Á¾±â

    [SerializeField]
    float m_PusDamage;

    protected override void MovePattern()
    {
        
    }



    public void Explosion()
    {
        GameObject Pus = ObjectPoolManager.instance.Get(m_Pus, transform.position);
        HitDetection Pus_HD = Pus.GetComponent<HitDetection>();
        Pus_HD.SetDamage(m_PusDamage, 20);
        Pus_HD.SetDisableTime(1,ENABLE_TYPE.Time);
        AudioManager.instance.SFXPlay(enemyAudio.attack);
        Dead();
    }

}
