using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    private AudioSource source;
    [field:SerializeField] int SessionID;              // 사운드 인스턴스마다 고유한 세션 ID (풀링될 때마다 증가)

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (source.clip != null && !source.isPlaying)
        {
            AudioManager.ReturnObject(this);
        }
    }

    public AudioSource GetAudioSource()
    {
        return source;
    }

    // 풀링될 때만 증가
    public void UpdateSessionID() { ++SessionID; }
    public int GetSessionID() { return SessionID; }
}

// Only for Pooling SFX. (BGM, Later)
// if you need Sound Stop,
// reference SoundInstance struct when you play SFX, and call Stop() when you want to stop the sound.
// SoundInstance.Stop() will stop the sound and unbind it immediately.
public struct SoundInstance
{
    Sound m_SoundObject;
    int m_SessionID;

    public SoundInstance(Sound obj)
    {
        m_SoundObject = obj;
        m_SessionID = obj.GetSessionID();
    }

    // ★ 핵심: 내가 가진 ID와 실제 Sound 객체의 ID가 같아야만 "유효"함
    public bool IsValid => m_SoundObject != null && m_SoundObject.GetSessionID() == m_SessionID;

    public void Stop()
    {
        if (IsValid)
        {
            m_SoundObject.GetAudioSource().Stop();
            m_SoundObject = null;
        }
        else if(m_SoundObject != null && m_SoundObject.GetSessionID() != m_SessionID)
        {
            Debug.LogWarning("SoundInstance: Sound object is not Valid.");
        }
    }
}