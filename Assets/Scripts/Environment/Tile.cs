using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    bool placeable;
    public GameObject wall;
    private bool hasWall = false;

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
        wall.transform.position = gameObject.transform.position + new Vector3(0, 1, 0);
        wall.transform.parent = gameObject.transform;
        wall.AddComponent<Wall>();
        GameObject.Instantiate(wall);
        hasWall = true;
    }
    public bool HasWall()
    {
        return hasWall;
    }
}
