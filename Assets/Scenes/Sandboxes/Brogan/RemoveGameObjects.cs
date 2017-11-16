using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveGameObjects : MonoBehaviour {

	public float removalDistance = 1f;
	public Vector3 target = new Vector3(34f, 0f, 4f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject go in enemies) {
			if (removalDistance > (target - go.gameObject.transform.position).magnitude)
				Destroy (go);
				
		}
		
	}
}
