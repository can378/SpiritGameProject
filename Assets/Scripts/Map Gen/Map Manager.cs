using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<MapChapter> mapTemplates = new List<MapChapter>();

    //public Dictionary<int, MapChapter> chapterMapDict = new Dictionary<int, MapChapter>();
    private MapChapter currentChapter;

    void Start()
    {
        // 현재 챕터 불러오기       
        LoadChapter();
    }

    public void LoadChapter()
    {
        int nowChapter = DataManager.instance.userData.nowChapter-1;
        Debug.Log($"현재 챕터{nowChapter}");

        // 기존 데이터 정리 (메모리 최적화)
        Resources.UnloadUnusedAssets();

        // 새로운 챕터 로드
        currentChapter = mapTemplates[nowChapter];

        Debug.Log($"[MapManager] 챕터 {nowChapter} 로드 완료");
    }

    public GameObject GetDefaultMap(bool top, bool bottom, bool left, bool right)
    {
        if (currentChapter == null)
        {
            Debug.LogError("현재 로드된 챕터가 없다");
            return null;
        }

        return currentChapter.GetDefaultMap(top, bottom, left, right);
    }

    /*
    public GameObject GetRewardMap()
    {
        if (currentChapter == null)
        {
            Debug.LogError("현재 로드된 챕터가 없습니다!");
            return null;
        }
        return currentChapter.GetRewardMap();
    }

    public GameObject GetMissionMap()
    {
        if (currentChapter == null)
        {
            Debug.LogError("현재 로드된 챕터가 없습니다!");
            return null;
        }
        return currentChapter.GetMissionMap();
    }

    public GameObject GetBossMap()
    {
        if (currentChapter == null)
        {
            Debug.LogError("현재 로드된 챕터가 없습니다!");
            return null;
        }
        return currentChapter.GetBossMap();
    }
    */
}
