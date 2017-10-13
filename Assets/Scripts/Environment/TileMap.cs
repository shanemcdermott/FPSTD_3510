using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    protected GameObject[,] tileMap;
	private int Xsize;
	private int Zsize;


    public void setMapSize(int numX, int numZ)
    {
        tileMap = new GameObject[numX, numZ];
		Xsize = numX;
		Zsize = numZ;
    }

    public void setTileAt(int x, int z, GameObject tile)
    {
        tileMap[x, z] = tile;
    }

    public GameObject getTileAt(int x, int z)
    {
        return tileMap[x, z];
    }

	public int getWidth()
	{
		return Xsize;
	}

	public int getLength()
	{
		return Zsize;
	}

}
