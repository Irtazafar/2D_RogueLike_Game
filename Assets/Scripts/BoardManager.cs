using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
//Tells Random to use the Unity Engine random number generator.
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int column = 8;
    public int row = 8;

    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    public Transform boardHolder;

    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for(int i = 1;i<column-1;i++)
        {
            for(int j=1;j<row-1;j++)
            {
                gridPositions.Add(new Vector3(i, j, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for(int x=-1; x<column+1;x++)
        {
            for(int y = -1;y<row+1;y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (x == -1 || x == column || y == -1 || y == row)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];

        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        
        int objectCount = Random.Range(minimum, maximum + 1);

      
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();

       
        InitialiseList();

        
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

       
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

      
        int enemyCount = (int)Mathf.Log(level, 2f);

      
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

      
        Instantiate(exit, new Vector3(column - 1, row - 1, 0f), Quaternion.identity);
    }
}

