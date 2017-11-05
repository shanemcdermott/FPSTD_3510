using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IFocusable
{
    public GameObject wall;
    private GameObject placedWall;

    public void PlaceWall()
    {
        wall.transform.position = transform.position + new Vector3(0, 0.25f, 0);
        GameObject wallInstance = GameObject.Instantiate(wall);
        wallInstance.transform.parent = transform;
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
    public void onBeginFocus(GameObject wallToShow)
    {
        if (wallToShow.GetComponent<Wall>())
        {
            if (placedWall == null)
            {
                wallToShow.transform.position = transform.position + new Vector3(0, 0.25f, 0);
                wallToShow.SetActive(true);
            }
        }
        
    }
    public void onEndFocus(GameObject wallToHide)
    {
        wallToHide.SetActive(false);
    }
}
