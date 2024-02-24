using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze2 : MonoBehaviour
{
    public int width, height;
    public GameObject wallPrefab;
    public float wallLength = 1f;

    private int[,] maze;
    private List<Vector2> visitedCells = new List<Vector2>();

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        maze = new int[width, height];
        CreateMaze(0, 0);
        DrawMaze();
    }

    void CreateMaze(int x, int y)
    {
        visitedCells.Add(new Vector2(x, y));
        int[] neighbors = new int[] { 1, 2, 3, 4 };
        neighbors = Shuffle(neighbors);

        for (int i = 0; i < neighbors.Length; i++)
        {
            int newX = x;
            int newY = y;

            switch (neighbors[i])
            {
                case 1: // Up
                    newY += 2;
                    break;
                case 2: // Right
                    newX += 2;
                    break;
                case 3: // Down
                    newY -= 2;
                    break;
                case 4: // Left
                    newX -= 2;
                    break;
            }

            if (newX > 0 && newX < width && newY > 0 && newY < height && !visitedCells.Contains(new Vector2(newX, newY)))
            {
                maze[newX, newY] = 1; // Mark as visited
                maze[x + (newX - x) / 2, y + (newY - y) / 2] = 1; // Carve path

                CreateMaze(newX, newY);
            }
        }
    }

    void DrawMaze()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (maze[i, j] == 1)
                {
                    Instantiate(wallPrefab, new Vector3(i * wallLength, j * wallLength, 0), Quaternion.identity);
                }
            }
        }
    }

    int[] Shuffle(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
    }
}
