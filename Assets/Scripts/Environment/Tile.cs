using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IFocusable
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
        wall.transform.position = transform.position + new Vector3(0, 0.25f, 0);
        GameObject wallInstance = GameObject.Instantiate(wall);
        wallInstance.transform.parent = transform;
		wallInstance.transform.localScale = new Vector3(1, .5f, 1); //huh?
        placedWall = wallInstance;
        placedWall.SetActive(true);
    }
		
    public void DestroyWall()
    {
        GameObject.Destroy(placedWall);
    }
    public bool HasWall()
    {
        return placedWall != null;
    }
    public void onBeginFocus(PlayerController focuser)
    {
        GameObject wallToShow = focuser.transparentStuff[0];
        if (wallToShow.GetComponent<Wall>())
        {
            if (placedWall == null)
            {
                wallToShow.transform.position = transform.position + new Vector3(0, 0.25f, 0);
				wallToShow.transform.localScale = this.transform.localScale;
                wallToShow.SetActive(true);
            }
        }
    }
    public void onEndFocus(PlayerController focuser)
    {
        GameObject wallToHide = focuser.transparentStuff[0];

        wallToHide.SetActive(false);
        
    }
}
