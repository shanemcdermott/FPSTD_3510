using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

	protected GameObject[,] tileMap;

	//display gizmos
	public bool giz = true;

	private float tileWidth = 1f;

	//size of tile map
	private int xlen, zlen;

	//start point and target for pathfinding
	private int startx, startz, targetx, targetz;

	//path through tileMap
	private Node[] path = null;
	private bool[,] isPath = null;

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
			//this heuristic only really makes sense if you can only move in cardinal directions
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

	public void Start()
	{
		//register self with the game manager
		if (GameManager.instance != null)
			GameManager.instance.tileMap = this;
	}

	public void initTileMap(int xSize, int zSize)
	{
		xlen = xSize;
		zlen = zSize;
		tileMap = new GameObject[zlen, xlen];

		setStartTile(0, 0);
		setTargetTile(xlen - 1, zlen - 1);

		path = null;
		isPath = new bool[zlen, xlen];
	}

	public bool findPath()
	{
		bool[,] psudoGrid = getBoolGridFromTileMap ();
		path = AStar (psudoGrid, startx, startz, targetx, targetz);

		isPath = new bool [zlen, xlen];
		foreach (Node n in path)
			isPath [n.zpos, n.xpos] = true;
		
		return path != null;
	}
		

	//translates the tile map into an array of booleans
	private bool [,]  getBoolGridFromTileMap()
	{
		bool [,] psudoGrid = new bool[zlen, xlen];
		for (int i = 0; i < xlen; i++) {
			for (int j = 0; j < zlen; j++) {
				if (tileMap [j, i].GetComponent<Tile> ().HasWall()) {
					psudoGrid [j, i] = false;
				} else {
					psudoGrid [j, i] = true;
				}
			}
		}

		return psudoGrid;
	}
		

	//preforms A* on the boolean grid passed to it trying to get from (sz, sx) to (tz, tx)
	private Node[] AStar(bool [,] grid, int sx, int sz, int tx, int tz)
	{
		//get dimentions of grid passed in
		int xlen = grid.GetLength (1);
		int zlen = grid.GetLength (0);

		//translate boolean grid to a grid of nodes
		Node[,] nodeGrid = new Node[zlen, xlen];
		for (int i = 0; i < xlen; i++) {
			for (int j = 0; j < zlen; j++) {
				if (grid[j,i] == false) {
					nodeGrid [j, i] = new Node(true);
				} else {
					nodeGrid [j, i] = new Node (i, j);
				}
			}
		}

		//create open list
		List<Node> open = new List<Node>(); //TODO: use something faster than a list

		//start with null path
		Node[] localPath = null;

		//set the cost of the start node to zero and add it to open list
		nodeGrid [sz, sx].costToGetHere = 0;
		open.Add (nodeGrid[sz, sx]);


		//while there are open nodes...
		while (open.Count > 0) {

			//find the smallest
			int smallest = 0;
			int smallestTotalCost = 1000000000; //something large
			for (int i = 0; i < open.Count; i++)
			{
				Node n = open [i];

				if (n.getEstimatedTotalCost(tx, tz) < smallestTotalCost) {
					smallest = i;
					smallestTotalCost = n.getEstimatedTotalCost(tx, tz);
				}
			}

			//if we are at the target
			if (open [smallest].xpos == tx && open [smallest].zpos == tz) {

				Node currPathNode = open [smallest];
				localPath = new Node[open [smallest].costToGetHere + 1];

				//trace the nodes that formed the path
				while (true) {
					localPath[currPathNode.costToGetHere] = currPathNode;

					if (currPathNode.fromz == -1 || currPathNode.fromx == -1) {
						return localPath;
					}
					currPathNode = nodeGrid [currPathNode.fromz, currPathNode.fromx];

				}
			}

			//try to add all four possible nodes (only cardinal directions)
			int[] nextZs = new int[4] { 1, -1, 0, 0 };
			int[] nextXs = new int[4] { 0, 0, 1, -1 };

			for (int i = 0; i < 4; i++) {
				int nextz = open [smallest].zpos + nextZs[i];
				int nextx = open [smallest].xpos + nextXs[i];

				//if node is within the array
				if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen) {
					//if not a wall
					if (!nodeGrid [nextz, nextx].isWall) {
						//if visiting this node creates a shorter path or (mostlikely) is an unvisited node
						if (nodeGrid [nextz, nextx].costToGetHere > open [smallest].costToGetHere + 1) {

							//set it's connection and cost and add it to the open list
							nodeGrid [nextz, nextx].costToGetHere = open [smallest].costToGetHere + 1;
							nodeGrid [nextz, nextx].setConnection (open [smallest].xpos, open [smallest].zpos);
							open.Add (nodeGrid [nextz, nextx]);
						}
					}
				}

			}

			open.Remove (open [smallest]);

		}

		return null;
	}
		

	//return the path but translated to realworld positions
	public Vector3[] getVector3Path()
	{
		if (path == null)
			return null;
		
		Vector3[] v3path = new Vector3[path.Length];

		for (int i = 0; i < path.Length; i++)
			v3path[i] = new Vector3 (path [i].xpos * tileWidth, 0, path [i].zpos * tileWidth); //this is temporary

		return v3path;
	}
		




	public bool HasWallHere(int x, int z)
	{
		return tileMap[z, x].GetComponent<Tile>().HasWall();
	}

	//place wall if it maintains a path through
	public void PlaceWallHere(int x, int z)
	{
		if (CanPlaceHere (x, z)) {
			tileMap [z, x].GetComponent<Tile> ().PlaceWall ();
			if (isPath[z, x])//only recalculate path if current path is blocked
				findPath ();
		}
	}

	public void DestroyWallHere (int x, int z)
	{
		tileMap [z, x].GetComponent<Tile> ().DestroyWall ();
		findPath ();//always update path
	}
		
	//place a wall without asking nicely
	public void forceWallHere(int x, int z)
	{
		tileMap [z, x].GetComponent<Tile> ().PlaceWall ();
	}


	//returns true as long as there is still a path through
	public bool CanPlaceHere(int x, int z)
	{
		//don't allow placement on start or target nodes
		if (x == startx && z == startz || x == targetx && z == targetz)
			return false;	
		
		//if there is already a wall there don't allow it
		if (getTileAt (x, z).GetComponent<Tile> ().HasWall ())
			return false;

		//if it doesn't block the path, allow it
		if (isPath[z, x] == false) 
			return true;
		
		//create a fake grid and add a wall to it
		bool[,] psudoGrid = getBoolGridFromTileMap ();
		psudoGrid [z, x] = false;

		//find the path if there is one and return
		Node[] tempPath = AStar (psudoGrid, startx, startz, targetx, targetz);
		return tempPath != null;
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


		

	public void OnDrawGizmos()
	{
		if (giz == true) {
			
			Vector3 cubeSize = new Vector3 (tileWidth, tileWidth, tileWidth);


			for (int x = 0; x < xlen; x++) {
				for (int z = 0; z < zlen; z++) {

					Vector3 center = new Vector3 (tileWidth * x, tileWidth * 0.5f, tileWidth * z);
					Gizmos.color = new Color (0.7f, 0.7f, 0.7f);

					if (tileMap [z, x].GetComponent<Tile>().HasWall()) {
						Gizmos.DrawCube (center, cubeSize);
					} else {
						Gizmos.DrawWireCube (center, cubeSize);
					}
				}
			}

//			if (path != null) {
//				for (int i = 0; i < path.Length; i++) {
//					Vector3 center = new Vector3 (tileWidth * path [i].xpos, tileWidth * 0.5f, tileWidth * path [i].zpos);
//					Gizmos.color = new Color (1f, 0f, 0f);
//					Gizmos.DrawCube (center, cubeSize / 2);
//					Gizmos.color = new Color (0.7f, 0.7f, 0.7f);
//				}
//			}
			Vector3[] v3path = getVector3Path();
			if (v3path != null) {
				for (int i = 0; i < v3path.Length; i++) {
					Gizmos.color = new Color (1f, 1f, 0f);
					Gizmos.DrawCube (v3path[i], cubeSize / 2);
					Gizmos.color = new Color (0.7f, 0.7f, 0.7f);
				}
			}
		}
	}
			
}