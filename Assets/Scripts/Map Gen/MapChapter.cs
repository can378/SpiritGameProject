using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMapChapter", menuName = "Map/MapChapter")]
public class MapChapter : ScriptableObject
{
    public string ChapterName;
    public DefaultMapWay[] DefaultMap;
    public GameObject[] RewardMap;
    public GameObject[] MissionMap;
    public GameObject[] BossMap;

    public GameObject GetDefaultMap(bool top, bool bottom, bool left, bool right)
    {
        int bitNumber = (Convert.ToByte(top) << 0) |
                        (Convert.ToByte(bottom) << 1) |
                        (Convert.ToByte(left) << 2) |
                        (Convert.ToByte(right) << 3);

        if (DefaultMap == null || DefaultMap.Length <= bitNumber || DefaultMap[bitNumber].References.Length == 0)
        {
            Debug.LogError($"DefaultMap 생성되지 않음 ({ChapterName})");
            return null;
        }

        return DefaultMap[bitNumber].References[UnityEngine.Random.Range(0, DefaultMap[bitNumber].References.Length)];
    }
}
