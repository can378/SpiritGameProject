using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class ChapterTile
{
    public TileBase[] swapTileBase;
}

public class ChangeTileSprite : MonoBehaviour
{
    public Tilemap m_Tilemap;

    int m_PrevChpterIndex;
    public int m_ChapterIndex;
    public ChapterTile[] swapChapter;

    void Update()
    {
        if (m_PrevChpterIndex != m_ChapterIndex)
        {
            for (int i = 0; i < swapChapter[m_ChapterIndex].swapTileBase.Length; ++i)
            {
                m_Tilemap.SwapTile(swapChapter[m_PrevChpterIndex].swapTileBase[i], swapChapter[m_ChapterIndex].swapTileBase[i]);
            }

            m_PrevChpterIndex = m_ChapterIndex;
        }
    }
}
