﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A Tower is a defense point*/

public class Tower : MonoBehaviour
{

    TowerHealth health;
    Team team;

    // Use this for initialization
    void Awake ()
    {
        health = GetComponent<TowerHealth>();
        team = GetComponent<Team>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
