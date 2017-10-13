using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : MonoBehaviour
{
	
	public GameObject target;

	public float speed = 4f;

	public bool alive;

	void Start ()
	{
		alive = true;
	}

	void Update ()
	{
		if (alive && target != null) {

			float zDiff = target.transform.position.z - this.transform.position.z;
			float xDiff = target.transform.position.x - this.transform.position.x;
			this.transform.localEulerAngles = new Vector3 (0, (Mathf.Atan2(xDiff, zDiff) / Mathf.PI * 180), 0);
			this.transform.Translate (new Vector3 (0, 0, speed * Time.deltaTime));

		}
	}

	public void kill()
	{
		alive = false;
	}
}
