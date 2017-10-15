using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    bool placeable;
    public GameObject wall;
    private GameObject placedWall;
    /// <summary>
    /// Whether or not this tile blocks
    /// </summary>
    public bool blocksMovement;

    void Awake()
    {
        placeable = true;
    }

    public bool IsBlocking()
    {
        return blocksMovement;
    }
    public bool IsPlaceable()
    {
        return placeable;
    }
    public void PlaceWall()
    {
        Vector3 wallPos = gameObject.transform.position + new Vector3(0, 1, 0);
        placedWall = GameObject.Instantiate(wall, wallPos, Quaternion.identity);
        placedWall.transform.parent = gameObject.transform;
        Debug.Log("Placed Wall at " + wallPos.ToString());
    }
    public bool HasWall()
    {
        return placedWall != null;
    }
}
