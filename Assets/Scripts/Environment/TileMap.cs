using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

	protected GameObject[,] tileMap;

	//display gizmos
	public bool giz = true;

	//size of tile map
	private int xlen, zlen;

	//start point and target for pathfinding
	private int startx, startz, targetx, targetz;

	//used for pathfinding
	//private bool[,] psudoGrid = null;
	private bool[,] visited = null; //unused
	private bool[,] inPath = null; //unused

	private Node[,] grid = null;
	private Node[] path = null;

	struct Node
	{
		public int xpos;
		public int zpos;
		public int costToGetHere;
		public int fromx;
		public int fromz;
		public bool isWall;
		int estimate;

		public Node (int x, int z)
		{
			xpos = x;
			zpos = z;
			costToGetHere = 10000;
			fromx = -1;
			fromz = -1;
			isWall = false;
			estimate = -1;

		}

		public Node (bool wall)
		{
			isWall = wall;
			xpos = -2;
			zpos = -2;
			costToGetHere = 10000;
			fromx = -2;
			fromz = -2;
			estimate = -1;
		}
			
		public int getEstimatedTotalCost(int tx, int tz)
		{
			if (estimate == -1)
				estimate = (Mathf.Abs (tx - xpos) + Mathf.Abs (tz - zpos));
			return estimate + costToGetHere;
		}

		public void setConnection(int x, int z)
		{
			fromx = x;
			fromz = z;
		}
			
		public void debugPrint()
		{
			Debug.Log ("Node at (x=" + xpos + ", z=" + zpos + ")\nisWall=" + isWall + "\nCost to get here: " + costToGetHere + "\nEstimated cost to target: " + estimate + "\nfrom node at (x=" + fromx + ", z=" + fromz + ")");
		}

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

    public void Start()
    {
        //register self with the game manager
		if (GameManager.instance != null)
       		GameManager.instance.tileMap = this;
    }

	private void translateMapToGridOfNodes()
	{
		grid = new Node[zlen, xlen];
		for (int i = 0; i < xlen; i++) {
			for (int j = 0; j < zlen; j++) {
				if (tileMap [j, i].GetComponent<Tile> ().HasWall()) {
					grid [j, i] = new Node(true);
				} else {
					grid [j, i] = new Node (i, j);
				}
			}
		}
	}


	public bool findPath()
	{
		visited = new bool[zlen, xlen];
		
		//initialize the grid (doing this every time might be costly)
		translateMapToGridOfNodes();
			

		//create open list
		List<Node> open = new List<Node>(); //TODO: use something faster than a list

		//reset path
		path = null;

		//set the cost of the start node to zero and add it to open list
		grid [startz, startx].costToGetHere = 0;
		open.Add (grid[startz, startx]);


		//while there are open nodes...
		while (open.Count > 0) {

			//find the smallest
			int smallestTotalCost = 1000000000;
			int smallest = 0;
			for (int i = 0; i < open.Count; i++)
			{
				Node n = open [i];

				if (n.getEstimatedTotalCost(targetx, targetz) < smallestTotalCost) {
					smallest = i;
					smallestTotalCost = n.getEstimatedTotalCost(targetx, targetz);
				}
			}

			//if we are at the target
			if (open [smallest].xpos == targetx && open [smallest].zpos == targetz) {

				Node currPathNode = open [smallest];
				path = new Node[open [smallest].costToGetHere + 1];

				//trace the nodes that formed the path
				while (true) {
					path[currPathNode.costToGetHere] = currPathNode;

					if (currPathNode.fromz == -1 || currPathNode.fromx == -1) {
						return true;
					}
					currPathNode = grid [currPathNode.fromz, currPathNode.fromx];

				}
			}
				
			//try to add all four possible nodes (only cardinal directions)
			int[] nextZs = new int[4] { 1, -1, 0, 0 };
			int[] nextXs = new int[4] { 0, 0, 1, -1 };

			for (int i = 0; i < 4; i++) {
				int nextz = open [smallest].zpos + nextZs[i];
				int nextx = open [smallest].xpos + nextXs[i];
				if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen) {
					grid [nextz, nextx] = grid [nextz, nextx];
					if (!grid [nextz, nextx].isWall) {
						if (grid [nextz, nextx].costToGetHere > open [smallest].costToGetHere + 1) {
							grid [nextz, nextx].costToGetHere = open [smallest].costToGetHere + 1;
							grid [nextz, nextx].setConnection (open [smallest].xpos, open [smallest].zpos);
							open.Add (grid [nextz, nextx]);
							visited [nextz, nextx] = true;
						}
					}
				}

			}

			open.Remove (open [smallest]);
			
		}

		Debug.Log ("No path found.");
		return false; //no path found

	}
		
	public void initTileMap(int xSize, int zSize)
	{
		xlen = xSize;
		zlen = zSize;
		tileMap = new GameObject[zlen, xlen];

		setStartTile(0, 0);
		setTargetTile(xlen - 1, zlen - 1);


		visited = new bool[zlen, xlen];
		inPath = new bool[zlen, xlen];

		path = null;
	}



	public bool HasWallHere(int x, int z)
	{
		return tileMap[z, x].GetComponent<Tile>().HasWall();
	}

	//place wall if it maintains a path through
	public void PlaceWallHere(int x, int z)
	{
		if (CanPlaceHere(x, z))
			tileMap [z, x].GetComponent<Tile> ().PlaceWall ();
		
	}

	public void DestroyWallHere (int x, int z)
	{
		tileMap [z, x].GetComponent<Tile> ().DestroyWall ();
	}
		
	//place a wall without asking nicely
	public void forceWallHere(int x, int z)
	{
		tileMap [z, x].GetComponent<Tile> ().PlaceWall ();
	}


	//returns true as long as there is still a path through
	public bool CanPlaceHere(int x, int z)
	{
		if (x == startx && z == startz || x == targetx && z == targetz)
			return false;	
		//maybe speed things up
//		if (inPath[z, x] == false) 
//			return true;
		if (getTileAt (x, z).GetComponent<Tile> ().HasWall ())
			return false;

		forceWallHere (x, z);


		if (findPath()) {
			return true;
		}
		else {
			getTileAt (x, z).GetComponent<Tile> ().DestroyWall ();
			Debug.Log ("Destroyed Wall!!!");
			Debug.Log(findPath ());
			//something wrong here...
			return false;

		}
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

					if (visited [z, x]) {
						Gizmos.color = new Color (0f, 1f, 1f);
						Gizmos.DrawWireCube (center, cubeSize);
						Gizmos.color = new Color (0.7f, 0.7f, 0.7f);
					}

					if (tileMap [z, x].GetComponent<Tile>().HasWall()) {
						Gizmos.DrawCube (center, cubeSize);
					} else {
						Gizmos.DrawWireCube (center, cubeSize);
					}
				}
			}

			if (path != null) {
				for (int i = 0; i < path.Length; i++) {
					Vector3 center = new Vector3 (tileWidth * path [i].xpos, tileWidth * 0.5f, tileWidth * path [i].zpos);
					Gizmos.color = new Color (1f, 0f, 0f);
					Gizmos.DrawCube (center, cubeSize / 2);
					Gizmos.color = new Color (0.7f, 0.7f, 0.7f);
				}
			}
		}
	}
			
}