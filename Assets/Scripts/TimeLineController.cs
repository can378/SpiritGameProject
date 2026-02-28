using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Playables;
using Unity.VisualScripting;

public class TimeLineController : MonoBehaviour
{
    [field:SerializeField]
    PlayableDirector m_PlayableDirector;
    [field: SerializeField]
    SerializedDictionary<string,PlayableAsset> m_Playable;  // 재생할 타임라인들을 딕셔너리로 관리 (키: 타임라인 이름, 값: PlayableAsset)

    public void Awake()
    {
        m_PlayableDirector.stopped += OnTimelineStopped;
    }

    public PlayState GetPlayableState()
    {
        return m_PlayableDirector.state;
    }

    // 컷신을 처음부터 재생
    public void Play(string _playableName)
    {
        this.gameObject.SetActive(true);

        if (!m_Playable.ContainsKey(_playableName))
        {
            Debug.LogError($"Playable '{_playableName}'이(가) m_Playable 딕셔너리에 존재하지 않습니다.");
            return;
        }
        
        m_PlayableDirector.playableAsset = m_Playable[_playableName];

        m_PlayableDirector.time = 0; // 타임라인을 처음부터 재생하도록 설정
        m_PlayableDirector.Play();
    }

    // 중지
    public void Stop()
    {
        m_PlayableDirector.Stop();
    }

    // 재개
    public void Resume()
    {
        m_PlayableDirector.Resume();
    }

    // 일시 정지
    public void Pause()
    {
        m_PlayableDirector.Pause();
    }

    public void SetPlayable(string _playableName)
    {
        if (!m_Playable.ContainsKey(_playableName))
        {
            Debug.LogError($"Playable '{_playableName}'이(가) m_Playable 딕셔너리에 존재하지 않습니다.");
            return;
        }

        Stop();

        m_PlayableDirector.playableAsset = m_Playable[_playableName];
    }

    // 재생 완료 시 비활성화
    void OnTimelineStopped(PlayableDirector aDirector)
    {
        if (m_PlayableDirector == aDirector)
        {
            Debug.Log("Timeline 재생 완료! 오브젝트를 끕니다.");
            gameObject.SetActive(false); // 혹은 특정 컴포넌트만 .enabled = false;
        }
    }

    // 파괴될 시 이벤트 구독 해제
    void OnDestroy()
    {
        m_PlayableDirector.stopped -= OnTimelineStopped;
    }


}

