using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeTileSprite : MonoBehaviour
{
    public Tilemap m_Tilemap;
    
    int m_PrevChpterIndex;
    public int m_ChapterIndex;
    public TileBase[] swapTileBase;

    void Update()
    {
        if(m_PrevChpterIndex != m_ChapterIndex)
        {
            m_Tilemap.SwapTile(swapTileBase[m_PrevChpterIndex], swapTileBase[m_ChapterIndex]);
            m_PrevChpterIndex = m_ChapterIndex;
        }
    }
}
