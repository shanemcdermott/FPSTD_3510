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
	public int startx = 4;
	public int startz = 4;
	public int targetx = 34;
	public int targetz = 4;

    /// <summary>
    /// Tile size in meters in X direction.
    /// </summary>
    public float tileWidth = 1f;
    /// <summary>
    /// Tile height in meters in Z direction.
    /// </summary>
    //public int tileHeight = 1;

    private Transform board;
    private TileMap map;

	// Use this for initialization
	void Start ()
    {
        //SetupScene();
	}

	public void SetupScene()
	{
		SetupBoard();
	}

    /// <summary>
    /// Populate map with random tiles.
    /// </summary>
    void SetupBoard()
    {
        GameObject boardObject = new GameObject("TileBoard");
        map = boardObject.AddComponent<TileMap>();
		map.transform.position = this.transform.position;
        board = boardObject.transform;

        //map.setMapSize(width, height);
		map.initTileMap(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
				GameObject toInstantiate = floorTiles[Random.Range (0, floorTiles.Length)];
				Vector3 p = this.transform.position;
				GameObject instance = Instantiate(toInstantiate, new Vector3(x * tileWidth + p.x, 0f + p.y, z * tileWidth + p.z), Quaternion.identity) as GameObject;
                instance.transform.SetParent(board);
				instance.transform.localScale = new Vector3(tileWidth, tileWidth, tileWidth);
				instance.GetComponent<Tile>().setCoordinates (x, z);
                map.setTileAt(x,z,instance);

            }
        }

		map.setTileWidth (tileWidth);
		map.setStartTile (startx, startz);
		map.setTargetTile (targetx, targetz);

		map.findPath (); 
		addBorder ();

		addBlockOfWalls (10, 0, 10, 10);
		//addBlockOfWalls (15, 5, 15, 14);
		//addBlockOfWalls (20, 0, 20, 10);
		addBlockOfWalls (25, 5, 25, 14);



		//addSomeWalls (wallsToAdd);



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

	public void addBlockOfWalls(int sx, int sz, int tx, int tz)
	{
		//s must be smaller than t!
		for (int x = sx; x <= tx; x++) {
			for (int z = sz; z <= tz; z++) {
				map.PlaceWallHere (x, z);
			}
		}
	}

	private void addBorder()
	{
		for (int i = 0; i < width; i++)
		{
			map.PlaceWallHere (i, 0);
			map.PlaceWallHere (i, height - 1);
		}

		for (int i = 0; i < height; i++)
		{
			map.PlaceWallHere (0, i);
			map.PlaceWallHere (width - 1, i);
		}

	}
 
}
