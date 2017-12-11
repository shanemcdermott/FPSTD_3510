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
	public bool [,]  getBoolGridFromTileMap()
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
	public Vector3[] getAStarPath(bool [,] grid, int sx, int sz, int tx, int tz)
	{
		return getVector3Path (AStar (grid, sx, sz, tx, tz));
	}

	//performs A* on the boolean grid passed to it trying to get from (sz, sx) to (tz, tx)
	private Node[] AStar(bool [,] grid, int sx, int sz, int tx, int tz)
	{
		//get dimentions of grid passed in
		int xlen = grid.GetLength (1);
		int zlen = grid.GetLength (0);

		if (sx > xlen || sx < 0 || sz > zlen || sz < 0 || tx > xlen || tx < 0 || tz > zlen || tz < 0) {
			Debug.Log ("Start and or target out of bounds");
			Debug.Log (sx + ", " + sz + ", " + ", " + tx + ", " + tz);
			return null;
		}


		//translate boolean grid to a grid of nodes
		Node[,] nodeGrid = new Node[zlen, xlen];
		for (int i = 0; i < xlen; i++) {
			for (int j = 0; j < zlen; j++) {
				if (grid[j,i] == false) {
					nodeGrid [j, i] = new Node(true);
				} else {
					nodeGrid [j, i] = new Node (i, j, (Mathf.Abs (tx - i) + Mathf.Abs (tz - j)));
				}
			}
		}

		//create open list
		NodePriorityQueue open = new NodePriorityQueue(); 

		//start with null path
		Node[] localPath = null;

		//set the cost of the start node to zero and add it to open list
		nodeGrid [sz, sx].costToGetHere = 0;
		open.push(nodeGrid[sz, sx]); 


		//while there are open nodes...
		while (open.getCount() > 0) { 

			Node n = open.pop(); 

			//if we are at the target
			if (n.xpos == tx && n.zpos == tz) {

				Node currPathNode = n;
				localPath = new Node[n.costToGetHere + 1];

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
				int nextz = n.zpos + nextZs[i];
				int nextx = n.xpos + nextXs[i];

				//if node is within the array
				if (nextz >= 0 && nextz < zlen && nextx >= 0 && nextx < xlen) {
					//if not a wall
					if (!nodeGrid [nextz, nextx].isWall) {
						//if visiting this node creates a shorter path or (mostlikely) is an unvisited node
						if (nodeGrid [nextz, nextx].costToGetHere > n.costToGetHere + 1) {

							//set it's connection and cost and add it to the open list
							nodeGrid [nextz, nextx].costToGetHere = n.costToGetHere + 1;
							nodeGrid [nextz, nextx].setConnection (n.xpos, n.zpos);
							open.push(nodeGrid [nextz, nextx]);
						}
					}
				}

			}

		}

		return null;
	}
		

	//return the path but translated to realworld positions
	public Vector3[] getVector3Path()
	{
		return getVector3Path (path); //if no param, use tileMaps path
	}

	public Vector3[] getVector3Path(Vector3 start, Vector3 target)
	{
		int sx, sz, tx, tz;
		nodeAtLocation (start, out sx, out sz);
		nodeAtLocation (target, out tx, out tz);
		return getVector3Path (AStar (getBoolGridFromTileMap (), sx, sz, tx, tz));
	}
		
	private Vector3[] getVector3Path(Node[] path)
	{
		if (path == null)
			return null;

		Vector3[] v3path = new Vector3[path.Length];

		for (int i = 0; i < path.Length; i++)
			v3path[i] = new Vector3 (path [i].xpos * tileWidth, 0, path [i].zpos * tileWidth) + gameObject.transform.position;

		return v3path;
	}
		
	//TODO conversion function for tiles and vector3s

	public void nodeAtLocation(Vector3 loc, out int x, out int z)
	{
		float xloc = (loc - gameObject.transform.position).x;
		float zloc = (loc - gameObject.transform.position).z;

		x = (int) Mathf.Floor (xloc / tileWidth);
		z = (int) Mathf.Floor (zloc / tileWidth);
		
		//set x and z to -1 if location is off grid
		if (x >= xlen || z >= zlen || x < 0 || z < 0) {
			x = -1;
			z = -1;
		}
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

		//always recalc the path after wall removal
		//findPath (); //this doesn't work for some reason, I think it's because of Destroy wall
		//workaround:
		bool[,] psudogrid = getBoolGridFromTileMap();
		psudogrid [z, x] = true;
		path = AStar(psudogrid, startx, startz, targetx, targetz);
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
		if (getTileAt (x, z).HasWall ())
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

	public void getStartTile(out int x, out int z)
	{
		x = startx;
		z = startz;
	}

	public void getTargetTile(out int x, out int z)
	{
		x = targetx;
		z = targetz;
	}

	public void setTileAt(int x, int z, GameObject tile)
	{
		tileMap[z, x] = tile;
	}

	public Tile getTileAt(int x, int z)
	{
		return tileMap[z, x].GetComponent<Tile>();
	}

	public int getWidth()
	{
		return xlen;
	}

	public int getLength()
	{
		return zlen;
	}

	public void setTileWidth(float width)
	{
		tileWidth = width;
	}

	public float getTileWidth()
	{
		return tileWidth;
	}


		

	public void OnDrawGizmos()
	{
		if (giz == true) {

			Vector3 cubeSize = new Vector3 (tileWidth, tileWidth, tileWidth);


			for (int x = 0; x < xlen; x++) {
				for (int z = 0; z < zlen; z++) {

					Vector3 center = new Vector3 (tileWidth * x, tileWidth * 0.5f, tileWidth * z) + this.transform.position;
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
//			Vector3[] v3path = getVector3Path();
//			if (v3path != null) {
//				for (int i = 0; i < v3path.Length; i++) {
//					Gizmos.color = new Color (1f, 1f, 0f);
//					Gizmos.DrawCube (v3path[i], cubeSize / 2);
//					Gizmos.color = new Color (0.7f, 0.7f, 0.7f);
//				}
//			}
		}
	}
			
}
