using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

	public bool giz = true;
    protected GameObject[,] tileMap;


	private int xlen;
	private int zlen;


	//start point and target for pathfinding
	private int startx;
	private int startz;
	private int targetx;
	private int targetz;

	//used for pathfinding
	private bool[,] grid = null;
	private bool[,] visited = null;
	private bool[,] path = null; //change this to a path


//    //do we need this?
//    public void setMapSize(int numX, int numZ)
//    {
//        tileMap = new GameObject[numX, numZ];
//		xlen = numX;
//		zlen = numZ;
//    }

	public void initTileMap(int xSize, int zSize)
	{
		xlen = xSize;
		zlen = zSize;
		tileMap = new GameObject[zlen, xlen];

		setStartTile(0, 0);
		setTargetTile(xlen - 1, zlen - 1);

		grid = new bool[zlen, xlen];
		visited = new bool[zlen, xlen];
		path = new bool[zlen, xlen];

		for (int x = 0; x < xlen; x++) {
			for (int z = 0; z < zlen; z++) {
				//grid [z, x] = !(getTileAt (x, z).GetComponent<Tile> ().HasWall ()); //false for can't walk through (maybe change this)
				grid[z, x] = true;
				visited[z, x] = false;
				path [z, x] = false;
			}
		}
	}

	void updateMap()
	{
		
	}

	public void setStartTile (int x, int z)
	{
		startx = x;
		startz = z;
	}

	public void setTargetTile (int x, int z)
	{
		targetx = x;
		targetz = z;
	}

    public void setTileAt(int x, int z, GameObject tile)
    {
        tileMap[z, x] = tile;
    }


    public GameObject getTileAt(int x, int z)
    {
        return tileMap[z, x];
    }

	public int getWidth()
	{
		return xlen;
	}

	public int getLength()
	{
		return zlen;
	}

	public bool HasWallHere(int x, int z)
	{
		return tileMap[z, x].GetComponent<Tile>().HasWall();
	}

	public void PlaceWallHere(int x, int z)
	{
		tileMap [z, x].GetComponent<Tile> ().PlaceWall ();
		isPath ();
	}

	public bool CanPlaceHere(int x, int z)
	{
		//check here if you can place...
		return true;
	}

	public bool isPath ()
	{
		
		return recursiveFindPath ();

	}

	public bool recursiveFindPath()
	{
		grid = new bool[zlen, xlen];
		visited = new bool[zlen, xlen];
		path = new bool[zlen, xlen];

		for (int x = 0; x < xlen; x++) {
			for (int z = 0; z < zlen; z++) {
				grid [z, x] = !(getTileAt (x, z).GetComponent<Tile> ().HasWall ()); //false for can't walk through (maybe change this)
				visited[z, x] = false;
				path [z, x] = false;
			}
		}

		return recursiveFindPathHelper(startx, startz);
	}

	public bool recursiveFindPathHelper(int x, int z)
	{
		visited [z, x] = true;

		if (x == targetx && z == targetz)
			return true;

		if (visitable (x, z + 1) && !visited[z + 1, x]) {
			if (recursiveFindPathHelper (x, z + 1)) {
				path [z + 1, x] = true;
				return true;
			}
		}

		if (visitable (x + 1, z) && !visited[z, x + 1]) {
			if (recursiveFindPathHelper (x + 1, z)) {
				path [z, x + 1] = true;
				return true;
			}
		}

		if (visitable (x - 1, z) && !visited[z, x - 1]) {
			if (recursiveFindPathHelper (x - 1, z)) {
				path [z, x -1] = true;
				return true;
			}
		}

		if (visitable (x, z - 1) && !visited[z - 1, x]) {
			if (recursiveFindPathHelper (x, z - 1)) {
				path [z - 1, x] = true;
				return true;
			}
		}


		return false;

	}

	private bool visitable(int x, int z)
	{
		if (!(x < 0 || x >= xlen || z < 0 || z >= zlen))
		{
			return grid [z, x];
		}
		return false;
	}



	public void OnDrawGizmos()
	{
		if (giz == true) {
			float tileWidth = 1f;
			Vector3 cubeSize = new Vector3 (tileWidth, tileWidth, tileWidth);


			for (int x = 0; x < xlen; x++) {
				for (int z = 0; z < zlen; z++) {


					Vector3 center = new Vector3 (tileWidth * x, tileWidth * 0.5f, tileWidth * z);
					Gizmos.color = new Color (0.7f, 0.7f, 0.7f);

					if (path != null && grid != null) {
						if (path [z, x] || (z == startz && x == startx) || (z == targetz && x == targetx)) {
							Gizmos.color = new Color (1f, 0f, 0f);
							Gizmos.DrawCube (center, cubeSize / 2);
							Gizmos.color = new Color (0.7f, 0.7f, 0.7f);
						}

						if (tileMap [z, x].GetComponent<Tile>().HasWall()) {
							Gizmos.DrawCube (center, cubeSize);
						} else {
							Gizmos.DrawWireCube (center, cubeSize);
						}
					}


				}
			}
		}
	}
			
}