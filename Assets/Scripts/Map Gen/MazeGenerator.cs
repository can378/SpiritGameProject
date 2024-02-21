using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public int width = 10;
    public int height = 10;
    public Vector2Int startCell;
    public Vector2Int endCell;

    private bool[,] visitedCells;
    private List<Vector2Int> directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        visitedCells = new bool[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                visitedCells[i, j] = false;
            }
        }

        CreateMazeFromCell(startCell.x, startCell.y);
    }

    void CreateMazeFromCell(int x, int y)
    {
        visitedCells[x, y] = true;

        Shuffle(directions);

        foreach (Vector2Int direction in directions)
        {
            int newX = x + direction.x * 2;
            int newY = y + direction.y * 2;

            if (newX >= 0 && newX < width && newY >= 0 && newY < height && !visitedCells[newX, newY])
            {
                // 벽 부수기
                Instantiate(wallPrefab, new Vector3(x + direction.x, y + direction.y, 0), Quaternion.identity);
                CreateMazeFromCell(newX, newY);
            }
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}
