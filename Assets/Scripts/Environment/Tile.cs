using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    /// <summary>
    /// Whether or not this tile blocks
    /// </summary>
    public bool blocksMovement;

    public bool IsBlocking()
    {
        return blocksMovement;
    }
}
