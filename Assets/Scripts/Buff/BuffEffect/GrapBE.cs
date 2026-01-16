using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapBE : BuffEffect
{
    BuffState m_BuffState;
    [field: SerializeField] Buff m_GrapBuff;
    bool m_PreLeftRight = true;
    [field: SerializeField] GameObject m_LKeySprite;
    [field: SerializeField] GameObject m_RKeySprite;
    [field: SerializeField] int m_KeyCount;
    [field: SerializeField] bool m_KeyLock;


    public override void Play(in Buff _Buff) 
    {
        m_GrapBuff = _Buff;
        m_BuffState = m_GrapBuff.m_BuffState;
        m_KeyCount = (int)m_GrapBuff.CustomData["KDC"];
        m_KeyLock = (bool)m_GrapBuff.CustomData["KeyLock"];
    }

    public override void Overlap(in Buff _Buff)
    {
        
    }

    void Update()
    {
        if (m_BuffState != BuffState.Apply)
            return;
        m_KeyLock = (bool)m_GrapBuff.CustomData["KeyLock"];
        if (m_KeyLock)
        {
            if(m_LKeySprite.gameObject.activeSelf || m_RKeySprite.gameObject.activeSelf)
            {
                m_LKeySprite.gameObject.SetActive(false);
                m_RKeySprite.gameObject.SetActive(false);
            }
            return;
        }

        bool LeftRight = (bool)m_GrapBuff.CustomData["LeftRight"];
        m_KeyCount = (int)m_GrapBuff.CustomData["KDC"];


        if (LeftRight == m_PreLeftRight)
            return;

        if (LeftRight)
        {
            m_PreLeftRight = LeftRight;
            m_LKeySprite.gameObject.SetActive(false);
            m_RKeySprite.gameObject.SetActive(true);
        }
        else
        {
            m_PreLeftRight = LeftRight;
            m_LKeySprite.gameObject.SetActive(true);
            m_RKeySprite.gameObject.SetActive(false);
        }
    }

    public override void Stop(in Buff _Buff)
    {
        m_BuffState = m_GrapBuff.m_BuffState;
        m_GrapBuff = null;

        m_LKeySprite.gameObject.SetActive(false);
        m_RKeySprite.gameObject.SetActive(false);

    }
}
