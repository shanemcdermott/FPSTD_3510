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
	int estimate; //this is an int... (float?)

	public string strname;

	public Node (int x, int z)
	{
		Debug.Log("Depricate this shit...");
		xpos = x;
		zpos = z;
		costToGetHere = 10000;
		fromx = -1;
		fromz = -1;
		isWall = false;
		estimate = -1;

		strname = "(" + xpos + ", " + zpos + ")";
	}

	public Node (int x, int z, int est)
	{
		xpos = x;
		zpos = z;
		costToGetHere = 10000;
		fromx = -1;
		fromz = -1;
		isWall = false;
		estimate = est;

		strname = "(" + xpos + ", " + zpos + ")";

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

		strname = "(wall.)";
	}

	public int estimatedTotalCost()
	{
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
