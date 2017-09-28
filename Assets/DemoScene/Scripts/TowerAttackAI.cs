using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackAI : MonoBehaviour {

	static public float timeBetweenShots = 0.5f;

	private float timeUntilNextShot = timeBetweenShots;

	void Start()
	{
		this.GetComponentInChildren<LineRenderer> ().enabled = false;
	}


	void FixedUpdate () {

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject player = GameObject.FindGameObjectWithTag ("Player");

		float closestDist = float.MaxValue;

		for (int i = 0; i < enemies.Length; i++) {
			float currDist = (player.transform.position - enemies[i].transform.position).sqrMagnitude;
			if (currDist < closestDist) {
				closestDist = currDist;
				this.GetComponent<TowerAim> ().setTarget (enemies [i]);
			}
			
			
		}

		timeUntilNextShot -= Time.deltaTime;

		if (timeUntilNextShot < 0) {
			StartCoroutine (shoot());
			timeUntilNextShot += timeBetweenShots;
		}

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
