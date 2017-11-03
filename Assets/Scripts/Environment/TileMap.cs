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
	//private bool[,] psudoGrid = null;
	private bool[,] visited = null;
	private bool[,] inPath = null;

	private Node[,] grid = null;
	private List<Node> path = null;

	struct Node
	{
		int xpos;
		int zpos;
		int costToGetHere;
		int fromx;
		int fromz;
		bool isWall;
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
				estimate = costToGetHere + (Mathf.Abs (tx - xpos) + Mathf.Abs (tz - zpos));
			return estimate;
		}

		public int getX()
		{
			return xpos;
		}

		public int getZ()
		{
			return zpos;
		}

		public int costSoFar()
		{
			return costToGetHere;
		}

		public void setCost(int cost)
		{
			costToGetHere = cost;
		}

		public void setConnection(int x, int z)
		{
			fromx = x;
			fromz = z;
		}

		public int getXConnection()
		{
			return fromx;
		}

		public int getZConnection ()
		{
			return fromz;
		}

		public bool hasWall()
		{
			return isWall;
		}

		public void debugPrint()
		{
			Debug.Log ("Node at (x=" + xpos + ", z=" + zpos + ")\nisWall=" + isWall + "\nCost to get here: " + costToGetHere + "\nEstimated cost to target: " + estimate + "\nfrom node at (x=" + fromx + ", z=" + fromz + ")");
		}

	}

    public void Start()
    {
        //register self with the game manager.
        //GameManager.instance.tileMap = this;
    }


	public bool findPath()
	{
		
		//initialize the grid (doing this every time might be costly)
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
			

		//create open and closed lists
		List<Node> open = new List<Node>(); //TODO: use a priority queue or something faster than a list
		//List<Node> closed = new List<Node> ();

		//set the cost of the start node to zero and add it to open list
		grid [startz, startx].setCost (0);
		open.Add (grid[startz, startx]);


		//while there are open nodes...
		while (open.Count > 0) {

			Debug.Log ("loop through open nodes");

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

			//Debug.Log ("found smallest");
			if (open [smallest].getX () == targetx && open [smallest].getZ () == targetz) {
				Debug.Log ("Arrived at goal Node!!!");

				path = new List<Node> ();
				while (true) {
					path.Add (open [smallest]);
					open [smallest].debugPrint ();
					if (open [smallest].getZConnection () == -1 || open [smallest].getXConnection () == -1) {
						Debug.Log ("Length of path: " + path.Count);
						for (int n = 0; n < path.Count; n++)
							path [n].debugPrint ();
						return true;
					}
					open [smallest] = grid [open [smallest].getZConnection (), open [smallest].getXConnection ()];

				}

				Debug.Log ("shouldn't ever get here");
				return true;
			}
				
			//try to add all four possible nodes
			//forward (z + 1)
			int nextz = open [smallest].getZ() + 1;
			int nextx = open [smallest].getX();
			if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen)
			{
				grid[nextz, nextx] = grid[nextz, nextx];
				if (!grid[nextz, nextx].hasWall())
				{
					if (grid[nextz, nextx].costSoFar () > open [smallest].costSoFar () + 1) {
						grid[nextz, nextx].setCost (open [smallest].costSoFar () + 1);
						grid[nextz, nextx].setConnection (open [smallest].getX (), open [smallest].getZ ());
						open.Add (grid[nextz, nextx]);
					}
				}
			}

			//backward (z - 1)
			nextz = open [smallest].getZ() - 1;
			nextx = open [smallest].getX();
			if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen)
			{
				grid[nextz, nextx] = grid[nextz, nextx];
				if (!grid[nextz, nextx].hasWall())
				{
					if (grid[nextz, nextx].costSoFar () > open [smallest].costSoFar () + 1) {
						grid[nextz, nextx].setCost (open [smallest].costSoFar () + 1);
						grid[nextz, nextx].setConnection (open [smallest].getX (), open [smallest].getZ ());
						open.Add (grid[nextz, nextx]);
					}
				}
			}

			//right (x + 1)
			nextz = open [smallest].getZ();
			nextx = open [smallest].getX() + 1;
			if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen)
			{
				grid[nextz, nextx] = grid[nextz, nextx];
				if (!grid[nextz, nextx].hasWall())
				{
					if (grid[nextz, nextx].costSoFar () > open [smallest].costSoFar () + 1) {
						grid[nextz, nextx].setCost (open [smallest].costSoFar () + 1);
						grid[nextz, nextx].setConnection (open [smallest].getX (), open [smallest].getZ ());
						open.Add (grid[nextz, nextx]);
					}
				}
			}

			//left (x - 1)
			nextz = open [smallest].getZ();
			nextx = open [smallest].getX() - 1;
			if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen)
			{
				grid[nextz, nextx] = grid[nextz, nextx];
				if (!grid[nextz, nextx].hasWall())
				{
					if (grid[nextz, nextx].costSoFar () > open [smallest].costSoFar () + 1) {
						grid[nextz, nextx].setCost (open [smallest].costSoFar () + 1);
						grid[nextz, nextx].setConnection (open [smallest].getX (), open [smallest].getZ ());
						open.Add (grid[nextz, nextx]);
					}
				}
			}

			//Debug.Log ("added candidates (hopefully)");
			open.Remove (open [smallest]);
			//closed.Add (open [smallest]);

			//Debug.Log ("moved current from open to closed");
			
		}

		Debug.Log ("No path found");
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

		path = new List<Node>();
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
		//maybe speed things up
		if (inPath[z, x] == false) 
			return true;
		if (getTileAt (x, z).GetComponent<Tile> ().HasWall ())
			return false;

		forceWallHere (x, z);


		if (findPath()) {
			return true;
		}
		else {
			getTileAt (x, z).GetComponent<Tile> ().DestroyWall ();
			Debug.Log ("Destroyed Wall!!!");
			findPath ();
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

					if (tileMap [z, x].GetComponent<Tile>().HasWall()) {
						Gizmos.DrawCube (center, cubeSize);
					} else {
						Gizmos.DrawWireCube (center, cubeSize);
					}
				}
			}

			for (int i = 0; i < path.Count; i++) {
				Vector3 center = new Vector3 (tileWidth * path[i].getX(), tileWidth * 0.5f, tileWidth * path[i].getZ());
				Gizmos.color = new Color (1f, 0f, 0f);
				Gizmos.DrawCube (center, cubeSize / 2);
				Gizmos.color = new Color (0.7f, 0.7f, 0.7f);
			}
		}
	}




	//getters and setters

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
			
}