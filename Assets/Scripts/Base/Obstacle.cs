using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    /*Can turrets be built on top of this obstacle?*/
    public bool bCanBuildUpon = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool CanBeBuiltUpon()
    {
        return bCanBuildUpon;
    }
}
