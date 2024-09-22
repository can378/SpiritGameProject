using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class ChapterTile
{
    public TileBase[] swapTileBase;
}

public class TileBaseTemplate : MonoBehaviour
{
    public ChapterTile[] swapChapter;
}
