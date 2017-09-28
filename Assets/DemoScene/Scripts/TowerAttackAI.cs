using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackAI : MonoBehaviour {

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
		
	}
}
