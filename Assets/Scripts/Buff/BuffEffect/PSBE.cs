using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 파티클을 사용하는 단순한 켜고 끄는 버프 이펙트
public class PSBE : BuffEffect
{
    ParticleSystem m_PS;

    private void Awake()
    {
        m_PS = GetComponent<ParticleSystem>();
    }

    public override void Play(in Buff _Buff)
    {
        m_PS.Play();
    }

    public override void Overlap(in Buff _Buff)
    {

    }
    public override void Stop(in Buff _Buff)
    {
        m_PS.Stop();
    }
}
