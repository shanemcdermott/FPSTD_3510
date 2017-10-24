using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject wall;
    private GameObject placedWall;

    public void PlaceWall()
    {
        wall.transform.position = transform.position + new Vector3(0, 0.25f, 0);
        GameObject wallInstance = GameObject.Instantiate(wall);
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
