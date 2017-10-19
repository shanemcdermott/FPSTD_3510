using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {


	//public TileMap map;

	public static float tileWidth = 1f;
	public Vector3 cubeSize = new Vector3 (tileWidth, tileWidth, tileWidth);


	public int startx;
	public int startz;
	public int targetx;
	public int targetz;

	public int xlen;
	public int zlen;

	private GameObject startIndicator;
	private GameObject targetIndicator;



	public bool[,] grid;
	private bool[,] visited;
	private bool[,] path;


	public void initGrid(int xl, int zl, float wallProb)
	{
		grid = new bool[xl, zl];
		startx = 0;
		startz = 0;
		xlen = xl;
		zlen = zl;

		targetx = xl - 1;
		targetz = zl - 1;

		System.Random r = new System.Random();


		for (int x = 0; x < xl; x++) {
			for (int z = 0; z < zl; z++) {
				if (r.NextDouble () > wallProb) {
					grid [z, x] = true; //can walk through
				} else {
					grid [z, x] = false; //can't walk through
				}
			}
		}

		grid [startz, startx] = true;
		grid [targetz, targetx] = true;

		visited = new bool[zlen, xlen];
		path = new bool[zlen, xlen];
		for (int x = 0; x < xlen; x++) {
			for (int z = 0; z < zlen; z++) {
				visited[z, x] = false;
				path [z, x] = false;
			}
		}

		Debug.Log ("Path found = " + recursiveFindPath (startx, startz));

	}

	public bool recursiveFindPath(int x, int z)
	{
		visited [z, x] = true;

		if (x == targetx && z == targetz)
			return true;
		
		if (visitable (x, z + 1) && !visited[z + 1, x]) {
			if (recursiveFindPath (x, z + 1)) {
				path [z + 1, x] = true;
				return true;
			}
		}

		if (visitable (x + 1, z) && !visited[z, x + 1]) {
			if (recursiveFindPath (x + 1, z)) {
				path [z, x + 1] = true;
				return true;
			}
		}

		if (visitable (x - 1, z) && !visited[z, x - 1]) {
			if (recursiveFindPath (x - 1, z)) {
				path [z, x -1] = true;
				return true;
			}
		}

		if (visitable (x, z - 1) && !visited[z - 1, x]) {
			if (recursiveFindPath (x, z - 1)) {
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

	public void Start()
	{
		initGrid (10, 10, 0.2f);
	}

	public void OnDrawGizmos()
	{
		for (int x = 0; x < xlen; x++) {
			for (int z = 0; z < zlen; z++) {
				Vector3 center = new Vector3 (tileWidth * x, tileWidth * 0.5f, tileWidth * z);
				Gizmos.color = new Color (0.7f, 0.7f, 0.7f);

				if (path[z, x] || (z == startz && x == startx)|| (z == targetz && x == targetx)) {
					Gizmos.color = new Color (1f, 0f, 0f);
					Gizmos.DrawCube (center, cubeSize / 2);
					Gizmos.color = new Color (0.7f, 0.7f, 0.7f);
				}
				if (grid[z, x]) {
					Gizmos.DrawWireCube (center, cubeSize);
				} else {
					Gizmos.DrawCube(center, cubeSize);
				}
			}
		}
	}































//	public void setMap(TileMap tm)
//	{
//		map = tm;
//
//
//		//temporary values, check if you can get from one corner to the other
//		startx = 0;
//		startz = 0;
//		targetx = map.getWidth() - 1;
//		targetz = map.getLength() - 1;
//
//
//		//put spheres where the start and target locations are
//		startIndicator = GameObject.CreatePrimitive (PrimitiveType.Sphere);
//		//startIndicator.GetComponent<Material> ().SetColor ("red", new Color(1f, 0f, 0f));
//		startIndicator.transform.position = map.getTileAt(startx, startz).transform.position + new Vector3(0f, 2f, 0f);
//		targetIndicator = GameObject.CreatePrimitive (PrimitiveType.Sphere);
//		//startIndicator.GetComponent<Material> ().SetColor ("red", new Color(1f, 0f, 0f));
//		targetIndicator.transform.position = map.getTileAt(targetx, targetz).transform.position + new Vector3(0f, 2f, 0f);
//
//	}
}
