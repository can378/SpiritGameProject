using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// πÊ ≈Î∑Œ
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
    public GameObject[] BossMap;

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
        int BitNumber = (Convert.ToByte(_Top) << 0) | (Convert.ToByte(_Bottom) << 1) | (Convert.ToByte(_Left) << 2) | (Convert.ToByte(_Right) << 3);
        return MissionMap[BitNumber].References[UnityEngine.Random.Range(0, MissionMap[BitNumber].References.Length)];
    }
}


