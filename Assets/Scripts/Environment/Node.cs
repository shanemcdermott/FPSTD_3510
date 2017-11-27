using System;
using UnityEngine;

public struct Node
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
