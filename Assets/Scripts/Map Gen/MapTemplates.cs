using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// �� ���
[System.Serializable]
public class DefaultMapWay
{
    public GameObject[] References;
}

public class MapTemplates : MonoBehaviour
{
    public DefaultMapWay[] DefaultMap = new DefaultMapWay[16];
    public DefaultMapWay[] RewardMap = new DefaultMapWay[16];
    public DefaultMapWay[] MissionMap = new DefaultMapWay[16];
    public DefaultMapWay[] BossMap = new DefaultMapWay[16];

    public GameObject GetDefaultMap(bool _Top, bool _Bottom, bool _Left, bool _Right)
    {
        int BitNumber = (Convert.ToByte(_Top) << 0) | (Convert.ToByte(_Bottom) << 1) | (Convert.ToByte(_Left) << 2) | (Convert.ToByte(_Right) << 3);
        return DefaultMap[BitNumber].References[UnityEngine.Random.Range(0, DefaultMap[BitNumber].References.Length)];
    }

    public GameObject GetRewardMap(bool _Top, bool _Bottom, bool _Left, bool _Right)
    {
        int BitNumber = (Convert.ToByte(_Top) << 0) | (Convert.ToByte(_Bottom) << 1) | (Convert.ToByte(_Left) << 2) | (Convert.ToByte(_Right) << 3);
        return RewardMap[BitNumber].References[UnityEngine.Random.Range(0, RewardMap[BitNumber].References.Length)];
    }

    public GameObject GetMissionMap(bool _Top, bool _Bottom, bool _Left, bool _Right)
    {
        int BitNumber = (Convert.ToByte(_Top) << 0)
                      | (Convert.ToByte(_Bottom) << 1)
                      | (Convert.ToByte(_Left) << 2)
                      | (Convert.ToByte(_Right) << 3);

        // �迭 ���� üũ
        if (BitNumber < 0 || BitNumber >= MissionMap.Length)
        {
            Debug.LogError($"BitNumber {BitNumber} is out of range for MissionMap (length={MissionMap.Length})");
            return null;
        }

        var references = MissionMap[BitNumber].References;

        // References �迭�� null�̰ų� ����ִ��� Ȯ��
        if (references == null || references.Length == 0)
        {
            Debug.LogWarning($"MissionMap[{BitNumber}].References is null or empty");
            return null;
        }

        return references[UnityEngine.Random.Range(0, references.Length)];
    }


    public GameObject GetBossMap(bool _Top, bool _Bottom, bool _Left, bool _Right)
    {
        int BitNumber = (Convert.ToByte(_Top) << 0) | (Convert.ToByte(_Bottom) << 1) | (Convert.ToByte(_Left) << 2) | (Convert.ToByte(_Right) << 3);
        return BossMap[BitNumber].References[UnityEngine.Random.Range(0, BossMap[BitNumber].References.Length)];
    }
}


