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
    public int width;
    /// <summary>
    /// Number of tiles in Z direction
    /// </summary>
    public int height;

	public int wallsToAdd = 10;

    /// <summary>
    /// Tile size in meters in X direction.
    /// </summary>
    public int tileWidth = 1;
    /// <summary>
    /// Tile height in meters in Z direction.
    /// </summary>
    public int tileHeight = 1;

    private Transform board;
    private TileMap map;

	// Use this for initialization
	void Start ()
    {
        SetupScene();
	}

	public void SetupScene()
	{
		SetupBoard();
		//this.GetComponentInChildren<PathFinder> ().setMap (this.map);
	}

    /// <summary>
    /// Populate map with random tiles.
    /// </summary>
    void SetupBoard()
    {
        GameObject boardObject = new GameObject("TileBoard");
        map = boardObject.AddComponent<TileMap>();
        board = boardObject.transform;

        //map.setMapSize(width, height);
		map.initTileMap(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
				GameObject toInstantiate = floorTiles[Random.Range (0, floorTiles.Length)];
				Vector3 p = this.transform.position;
				GameObject instance = Instantiate(toInstantiate, new Vector3(x * tileWidth + p.x, 0f + p.y, z * tileHeight + p.z), Quaternion.identity) as GameObject;
                instance.transform.SetParent(board);
				instance.GetComponent<Tile>().setCoordinates (x, z);
                map.setTileAt(x,z,instance);

            }
        }

		addSomeWalls (wallsToAdd);



    }

	public TileMap getTileMap()
	{
		return map;
	}

	public void addSomeWalls(int numWalls)
	{
		for (int i = 0; i < numWalls; i++) {
			int x = Random.Range (0, width);
			int z = Random.Range (0, height);

			map.PlaceWallHere (x, z);
		}
	}
 
}
