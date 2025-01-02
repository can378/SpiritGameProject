using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공용으로 쓸만한 함수, 클래스 모음


/// <summary>
/// 가중치 확률 함수
/// </summary>
/// <typeparam name="T">종류의 타입</typeparam>
[System.Serializable]
public class WeightRandom<T>
{
    // 출처 
    // https://unity-programming-study.tistory.com/9

    [field : SerializeField]
    Dictionary<T, int> m_Dic = new Dictionary<T, int>();           // 가중치

    public void Add(T _Key, int _Value)
    {
        if(_Value < 0)
            return;

        if(m_Dic.ContainsKey(_Key))
            m_Dic[_Key] = _Value;

        m_Dic.Add(_Key,_Value);
    }

    public void Sub(T _Key, int _Value)
    {
        if(_Value < 0)
        {
            return;
        }

        if(m_Dic.ContainsKey(_Key))
        {
            if(m_Dic[_Key] > _Value)
                m_Dic[_Key] -= _Value;
            else 
                Remove(_Key);
        }
    }

    public void Remove(T _Key)
    {
        // 해당 키가 존재 하지 않으면 반환
        if(!m_Dic.ContainsKey(_Key))
            return;
        
        m_Dic.Remove(_Key);
    }

    public int GetTotalWeight()
    {
        int totalWeight = 0;
        foreach(int value in m_Dic.Values)
        {
            totalWeight += value;
        }

        return totalWeight;
    }

    public Dictionary<T, float> GetPercent()
    {
        Dictionary<T,float> _tempDic = new Dictionary<T,float>();
        float totalWeight = GetTotalWeight();

        foreach(var item in m_Dic)
        {
            _tempDic.Add(item.Key, item.Value/totalWeight);
        }

        return _tempDic;
    }

    public T GetRandomItem()
    {
        if(m_Dic.Count <= 0)
        {
            return default;
        }

        int totalWeight = GetTotalWeight();
        int weight = 0;

        int pivot = Random.Range(0, totalWeight);

        foreach(var item in m_Dic)
        {
            weight += item.Value;
            if(pivot < weight)
                return item.Key;
        }

        return default;
    }
}

public static class CombinationRandom
{
    public static List<int> CombRandom(int length, int min, int max)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < length; i++)
        {
            while (true)
            {
                int ran = Random.Range(min, max);
                if (list.IndexOf(ran) == -1)
                {
                    list.Add(ran);
                    break;
                }
            }

        }
        return list;
    }
}

public interface Interactable
{
    string GetInteractText();
    void Interact();
}