using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 파티클을 사용하는 중첩 버프 이펙트
public class OverlapPSBE: BuffEffect
{

    ParticleSystem m_PS;
    [SerializeField] float m_RateOverTime;

    private void Awake()
    {
        m_PS = GetComponent<ParticleSystem>();
    }

    public override void Play(in Buff _Buff)
    {
        m_PS.Play();
        Overlap(_Buff);
    }

    public override void Overlap(in Buff _Buff)
    {
        // 방울이 더 많이
        var emission = m_PS.emission;
        emission.rateOverTime = m_RateOverTime * _Buff.stack;
    }
    public override void Stop(in Buff _Buff)
    {
        m_PS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
