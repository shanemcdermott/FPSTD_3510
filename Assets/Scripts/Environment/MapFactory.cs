using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFactory : MonoBehaviour
{
    /// <summary>
    /// Array of floor tile prefabs.
    /// </summary>
    public GameObject[] floorTiles;
    /// <summary>
    /// Array of wall tile prefabs.
    /// </summary>
    public GameObject[] wallTiles;

    /// <summary>
    /// Number of tiles in X direction
    /// </summary>
    public int width = 10;
    /// <summary>
    /// Number of tiles in Z direction
    /// </summary>
    public int height = 10;

    private Transform board;
    private GameObject[,] tileMap;

	// Use this for initialization
	void Start ()
    {
        SetupScene();
	}
	
    /// <summary>
    /// Populate map with random tiles.
    /// </summary>
    void SetupBoard()
    {
        board = new GameObject("Board").transform;
        tileMap = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, 0f, z), Quaternion.identity) as GameObject;
                instance.transform.SetParent(board);
                tileMap[x, z] = instance;
            }
        }
    }

    public void SetupScene()
    {
        SetupBoard();
    }
}
