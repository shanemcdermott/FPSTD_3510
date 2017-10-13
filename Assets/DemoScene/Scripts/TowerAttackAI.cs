﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackAI : MonoBehaviour {

    //The Tower's equipment/weapon
    public Equipment equipment;

    private GameObject player;
    private TowerAim towerAim;

	static public float timeBetweenShots = 0.5f;

	private float timeUntilNextShot = timeBetweenShots;

	void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player");
        towerAim = this.GetComponent<TowerAim>();
        if (equipment == null)
            equipment = this.GetComponentInChildren<Equipment>();
        //this.GetComponentInChildren<LineRenderer> ().enabled = false;
	}

    protected GameObject FindTargetClosestTo(Vector3 position)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject target = null;
        float closestDist = float.MaxValue;

        for (int i = 0; i < enemies.Length; i++)
        {
            float currDist = (position - enemies[i].transform.position).sqrMagnitude;
            if (currDist < closestDist)
            {
                closestDist = currDist;
                target = enemies[i];
            }
        }
        return target;
    }

	void FixedUpdate ()
    {
        GameObject target = FindTargetClosestTo(player.transform.position);
        towerAim.setTarget(target);

        if (target != null)
            equipment.Activate();

	}


	private IEnumerator shoot()
	{
		this.GetComponentInChildren<LineRenderer> ().enabled = true; //this might not be the best way to do this...
		yield return new WaitForSeconds (0.2f);
		Component targetComponent =  this.GetComponent<TowerAim> ().getTarget ().GetComponent<ReactiveTarget> ();
		if (targetComponent != null)
			this.GetComponent<TowerAim> ().getTarget ().GetComponent<ReactiveTarget> ().reactToHit ();
		this.GetComponentInChildren<LineRenderer> ().enabled = false;
	}
}
