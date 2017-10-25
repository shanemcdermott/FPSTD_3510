using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject wall;
    private GameObject placedWall;

	private int xPos = -1;
	private int zPos = -1;

	public TileMap getParentTileMap()
	{
		return this.GetComponentInParent<TileMap> ();
	}

	public int getXPos()
	{
		return xPos;
	}

	public int getZPos()
	{
		return zPos;
	}

	public void setCoordinates(int x, int z)
	{
		xPos = x;
		zPos = z;
	}

    public void PlaceWall()
    {
		//TODO: need to check if wall can be placed here
		wall.transform.position = transform.position + new Vector3 (0, 0.25f, 0);
		GameObject wallInstance = GameObject.Instantiate (wall);
		wallInstance.transform.parent = transform;
		placedWall = wallInstance;
    }
		
    public void DestroyWall()
    {
        GameObject.Destroy(placedWall);
    }
    public bool HasWall()
    {
        return placedWall != null;
    }
}
