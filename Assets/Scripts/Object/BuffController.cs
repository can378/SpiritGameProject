using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;


public class BuffController : MonoBehaviour
{
    [SerializeField] ObjectBasic m_Owner;
    [field: SerializeField] public SerializedDictionary<int, Buff> m_activeEffects = new SerializedDictionary<int, Buff>();         //버프 디버프

    public event System.Action m_BuffApplyEvent;
    public event System.Action m_BuffOverlapEvent;
    public event System.Action m_BuffRemoveEvent;

    public List<ParticleSystem> m_BuffEffectList = new List<ParticleSystem>();


    // 비활성화로 버프 기능을 끄고 켤수 있다.
    void Update()
    {
        Update_Buff();
    }

    public Buff ApplyBuff(BuffData _Buff)
    {
        if (m_Owner.status.isDead)
            return null;

        Buff buff;
        m_activeEffects.TryGetValue(_Buff.buffID, out buff);

        // 이미 같은 버프가 있다면 중첩 처리
        if (buff != null)
        {
            buff.Overlap();
            m_BuffOverlapEvent?.Invoke();
            OverlapEffect(buff);
            return buff;
        }

        buff = new Buff(_Buff, m_Owner);

        buff.Apply();
        m_BuffApplyEvent?.Invoke();
        m_activeEffects.Add(_Buff.buffID, buff);

        // 버프 이펙트를 켠다.
        ActvieEffect(_Buff.buffID,true);
        OverlapEffect(buff);


        return buff;
    }

    public Buff FindBuff(BuffData _Buff)
    {
        Buff buff;
        m_activeEffects.TryGetValue(_Buff.buffID, out buff);
        return buff;
    }

    public void RemoveBuff(BuffData _Buff)
    {
        Buff buff;
        m_activeEffects.TryGetValue(_Buff.buffID, out buff);

        if (buff == null)
        {
            Debug.LogWarning("버프가 존재하지 않습니다.");
            return;
        }

        buff.Remove();                                      // 버프 해제
        m_BuffRemoveEvent?.Invoke();
        //Destroy(buff.gameObject);                         // 버프 아이콘 삭제
        m_activeEffects.Remove(_Buff.buffID);               // 리스트에서 제거

        ActvieEffect(_Buff.buffID, false);

    }

    public void RemoveBuff(int _ID)
    {
        Buff buff;
        m_activeEffects.TryGetValue(_ID, out buff);

        if (buff == null)
        {
            Debug.LogWarning("버프가 존재하지 않습니다.");
            return;
        }

        buff.Remove();                                      // 버프 해제
        m_BuffRemoveEvent?.Invoke();
        //Destroy(buff.gameObject);                         // 버프 아이콘 삭제
        m_activeEffects.Remove(_ID);               // 리스트에서 제거

        ActvieEffect(_ID, false);

    }

    protected void Update_Buff()
    {
        if (m_Owner.status.isDead)
            return;

        List<int> toRemove = new();

        foreach (var kvp in m_activeEffects)
        {
            // 지속 시간 종료 시
            Buff buff = kvp.Value;
            if (0 >= buff.curDuration)
            {
                m_activeEffects[buff.buffData.buffID].Remove();                // 버프 해제
                //Destroy(stats.activeEffects[i].gameObject);     // 버프 아이콘 삭제
                toRemove.Add(buff.buffData.buffID);              // 리스트에서 제거
                continue;
            }
            buff.curDuration -= Time.deltaTime;  // 지속시간 감소
            buff.Update_Buff();                  // 효과
        }

        foreach (int id in toRemove)
        {
            RemoveBuff(id);
        }
    }

    public void RemoveAllBuff()
    {
        foreach (var kvp in m_activeEffects)
        {
            Buff buff = kvp.Value;
            buff.Remove();
            //Destroy(effect.gameObject);
        }
        m_activeEffects.Clear();
    }

    void ActvieEffect(int _BuffID, bool _Active)
    {

        if (_BuffID < 0 || _BuffID >= m_BuffEffectList.Count)
            return;
        if(m_BuffEffectList[_BuffID] == null)
            return;

        if (_Active)
        {
            m_BuffEffectList[_BuffID].Clear();
            m_BuffEffectList[_BuffID].Play();
        }
        else
            m_BuffEffectList[_BuffID].Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    // 버프 수가 많지 않으므로 우선 스위치문으로 하드 코딩
    // 종류가 많아진다면 BuffeEffectController, BuffeEffect 클래스를 만들 예정
    void OverlapEffect(Buff _Buff)
    {
        switch(_Buff.buffData.buffID)
        {
            case 4: //Poison
                PoisonStackEffect(_Buff);
                break;
            case 8: //Bleeding
                BleedingStackEffect(_Buff);
                break;
            default:
                break;
        }
    }

    // 중첩별 이펙트 효과 변경
    void PoisonStackEffect(Buff _Buff)
    {
        // 방울이 더 많이
        var emission = m_BuffEffectList[_Buff.buffData.buffID].emission;
        emission.rateOverTime = 4 * _Buff.stack;
    }

    void BleedingStackEffect(Buff _Buff)
    {
        // 방울이 더 많이
        var emission = m_BuffEffectList[_Buff.buffData.buffID].emission;
        emission.rateOverTime = 0.5f * _Buff.stack;
    }

}
