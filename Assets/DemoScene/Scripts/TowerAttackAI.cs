﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackAI : MonoBehaviour {

    //The Tower's equipment/weapon
    public Weapon equipment;

    private GameObject defenseTarget;
    private TowerAim towerAim;

	static public float timeBetweenShots = 0.5f;

	private float timeUntilNextShot = timeBetweenShots;
  
	void Start()
	{
        towerAim = this.GetComponent<TowerAim>();
        if (equipment == null)
        {
            //equipment = this.GetComponentInChildren<Weapon>();
            equipment.SetCurrentState(WeaponState.Idle);
        }

        defenseTarget = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("defense target: " + defenseTarget + "parent: " + gameObject.name);
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
                Debug.Log("target aquired: " + target.name + " at: " + target.transform.position);
            }
        }
        return target;
    }

	void FixedUpdate ()
    {
        GameObject target = FindTargetClosestTo(defenseTarget.transform.position);
        towerAim.setTarget(target);

        equipment.aimTransform = gameObject.transform;
        if (target != null)
        {
            Debug.Log("Name:" + gameObject.name + "Transform: " + gameObject.transform.localEulerAngles + " local: " + gameObject.transform.localRotation);
            Shoot();
        }
        /*
        if (target != null && equipment != null && equipment.CanActivate())
            equipment.Activate();
        */
	}


	private void Shoot()
	{
        if (towerAim.getTarget() != null && equipment != null)
        {
            Debug.Log("activating weapon");
            equipment.Activate();
        }
        /*
		this.GetComponentInChildren<LineRenderer> ().enabled = true; //this might not be the best way to do this...
		yield return new WaitForSeconds (0.2f);
		Component targetComponent =  this.GetComponent<TowerAim> ().getTarget ().GetComponent<ReactiveTarget> ();
		if (targetComponent != null)
			this.GetComponent<TowerAim> ().getTarget ().GetComponent<ReactiveTarget> ().reactToHit ();
		this.GetComponentInChildren<LineRenderer> ().enabled = false;
        */
	}
    
}
