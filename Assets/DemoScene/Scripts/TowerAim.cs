using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAim : MonoBehaviour {

	public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float zDiff = target.transform.position.z - this.transform.position.z;
		float yDiff = target.transform.position.y - this.transform.position.y;
		float xDiff = target.transform.position.x - this.transform.position.x;
		float xAngle = (Mathf.Atan2 (Mathf.Abs(yDiff), Mathf.Sqrt(xDiff * xDiff + zDiff * zDiff)) / Mathf.PI * 180);
		float yAngle = (Mathf.Atan2 (xDiff, zDiff) / Mathf.PI * 180);



		this.transform.localEulerAngles = new Vector3 (xAngle, yAngle, 0);

	}

	public void setTarget(GameObject go)
	{
		target = go;
	}
}
