﻿using System.Collections;
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

	//path that can be given to enemies etc...
	private Vector3[] path;


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
			redoPathfindingData ();
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