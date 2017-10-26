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
	private bool[,] psudoGrid = null;
	private bool[,] visited = null;
	private bool[,] inPath = null;

	private Node[,] psudoGridTwo = null;
	private List<Node> path = null;

	struct Node
	{
		int xpos;
		int zpos;
		int costToGetHere;
		int fromx;
		int fromz;
		bool isWall;

		public Node (int x, int z)
		{
			xpos = x;
			zpos = z;
			costToGetHere = 10000;
			fromx = -1;
			fromz = -1;
			isWall = false;

		}

		public Node (bool wall)
		{
			isWall = wall;
			xpos = -1;
			zpos = -1;
			costToGetHere = 10000;
			fromx = -1;
			fromz = -1;
		}
			
		public int getEstimatedTotalCost(int tx, int tz)
		{
			//TODO: store estimated calculated value for efficiency
			return costToGetHere + (Mathf.Abs (tx - xpos) + Mathf.Abs (tz - zpos));
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
	}




	public bool findPath()
	{
		
		//initialize the grid (doing this every time might be costly)
		psudoGridTwo = new Node[zlen, xlen];
		for (int i = 0; i < xlen; i++) {
			for (int j = 0; j < zlen; j++) {
				if (tileMap [j, i].GetComponent<Tile> ().HasWall()) {
					psudoGridTwo [j, i] = new Node(true);
				} else {
					psudoGridTwo [j, i] = new Node (i, j);
				}
			}
		}
			

		//create open and closed lists
		List<Node> open = new List<Node>(); //TODO: use a priority queue or something faster than a list
		List<Node> closed = new List<Node> ();

		//set the cost of the start node to zero and add it to open list
		psudoGridTwo [startz, startx].setCost (0);
		open.Add (psudoGridTwo[startz, startx]);


		Debug.Log ("init finished");

		//while there are open nodes...
		while (open.Count > 0) {

			Debug.Log ("start of loop through open");

			int smallestTotalCost = 1000000000;
			int indexOfSmallest = 0;
			for (int i = 0; i < open.Count; i++)
			{
				Node n = open [i];

				if (n.getEstimatedTotalCost(targetx, targetz) < smallestTotalCost) {
					indexOfSmallest = i;
					smallestTotalCost = n.getEstimatedTotalCost(targetx, targetz);
				}
			}

			Node current = open [indexOfSmallest];

			Debug.Log ("found smallest");
			if (current.getX () == targetx && current.getZ () == targetz) {
				Debug.Log ("Arrived at goal Node!!!");

				path = new List<Node> ();
				while (true) {
					path.Add (current);
					if (current.getZConnection () == -1 || current.getXConnection () == -1) {
						Debug.Log ("Length of path: " + path.Count);
						return true;
					}
					current = psudoGridTwo [current.getZConnection (), current.getXConnection ()];

				}

				//shouldn't ever get here
				return true;
			}

			//process this node
			Node candidate;

			//try to add all four possible nodes
			int nextz = current.getZ() + 1;
			int nextx = current.getX();
			if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen)
			{
				candidate = psudoGridTwo[nextz, nextx];
				if (!candidate.hasWall())
				{
					if (candidate.costSoFar () > current.costSoFar () + 1) {
						candidate.setCost (current.costSoFar () + 1);
						candidate.setConnection (current.getX (), current.getZ ());
						open.Add (candidate);
					}
				}
			}

			nextz = current.getZ() - 1;
			nextx = current.getX();
			if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen)
			{
				candidate = psudoGridTwo[nextz, nextx];
				if (!candidate.hasWall())
				{
					if (candidate.costSoFar () > current.costSoFar () + 1) {
						candidate.setCost (current.costSoFar () + 1);
						candidate.setConnection (current.getX (), current.getZ ());
						open.Add (candidate);
					}
				}
			}

			nextz = current.getZ();
			nextx = current.getX() + 1;
			if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen)
			{
				candidate = psudoGridTwo[nextz, nextx];
				if (!candidate.hasWall())
				{
					if (candidate.costSoFar () > current.costSoFar () + 1) {
						candidate.setCost (current.costSoFar () + 1);
						candidate.setConnection (current.getX (), current.getZ ());
						open.Add (candidate);
					}
				}
			}

			nextz = current.getZ();
			nextx = current.getX() - 1;
			if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen)
			{
				candidate = psudoGridTwo[nextz, nextx];
				if (!candidate.hasWall())
				{
					if (candidate.costSoFar () > current.costSoFar () + 1) {
						candidate.setCost (current.costSoFar () + 1);
						candidate.setConnection (current.getX (), current.getZ ());
						open.Add (candidate);
					}
				}
			}

			Debug.Log ("added candidates (hopefully)");
			open.Remove (current);
			closed.Add (current);

			Debug.Log ("moved current from open to closed");
			
		}

		return false; //no path found

	}







	public void initTileMap(int xSize, int zSize)
	{
		xlen = xSize;
		zlen = zSize;
		tileMap = new GameObject[zlen, xlen];

		setStartTile(0, 0);
		setTargetTile(xlen - 1, zlen - 1);

		psudoGrid = new bool[zlen, xlen];
		visited = new bool[zlen, xlen];
		inPath = new bool[zlen, xlen];
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


		if (recursiveFindPath ()) {
			return true;
		}
		else {
			getTileAt (x, z).GetComponent<Tile> ().DestroyWall ();
			Debug.Log ("Destroyed Wall!!!");
			recursiveFindPath ();
			return false;

		}
	}

	private void redoPathfindingData()
	{
		psudoGrid = new bool[zlen, xlen];
		visited = new bool[zlen, xlen];
		inPath = new bool[zlen, xlen];

		for (int x = 0; x < xlen; x++) {
			for (int z = 0; z < zlen; z++) {
				psudoGrid [z, x] = !(getTileAt (x, z).GetComponent<Tile> ().HasWall ());
				visited[z, x] = false;
				inPath [z, x] = false;
			}
		}
	}




		

	public bool recursiveFindPath()
	{
		redoPathfindingData ();

		if (psudoGrid [startz, startx] == false || psudoGrid [targetz, targetx] == false)
			return false;

		return recursiveFindPathHelper(startx, startz);
	}

	public bool recursiveFindPathHelper(int x, int z)
	{
		visited [z, x] = true;

		if (x == targetx && z == targetz)
			return true;

		if (visitable (x, z + 1) && !visited[z + 1, x]) {
			if (recursiveFindPathHelper (x, z + 1)) {
				inPath [z + 1, x] = true;
				return true;
			}
		}

		if (visitable (x + 1, z) && !visited[z, x + 1]) {
			if (recursiveFindPathHelper (x + 1, z)) {
				inPath [z, x + 1] = true;
				return true;
			}
		}

		if (visitable (x - 1, z) && !visited[z, x - 1]) {
			if (recursiveFindPathHelper (x - 1, z)) {
				inPath [z, x -1] = true;
				return true;
			}
		}

		if (visitable (x, z - 1) && !visited[z - 1, x]) {
			if (recursiveFindPathHelper (x, z - 1)) {
				inPath [z - 1, x] = true;
				return true;
			}
		}


		return false;

	}

	private bool visitable(int x, int z)
	{
		if (!(x < 0 || x >= xlen || z < 0 || z >= zlen))
		{
			return psudoGrid [z, x];
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

					if (inPath != null && psudoGrid != null) {
						if (inPath [z, x] || (z == startz && x == startx) || (z == targetz && x == targetx)) {
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