using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInput : MonoBehaviour
{
	private CharacterController _charController;
	public float speed = 40f;
	public float gravity = -9.8f;
	//umm....

	// Use this for initialization
	void Start ()
	{
		_charController = GetComponent <CharacterController> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float deltaX = Input.GetAxis ("Horizontal") * speed;
		float deltaZ = Input.GetAxis ("Vertical") * speed;

		Vector3 movement = new Vector3 (deltaX, 0, deltaZ);
		movement = movement.normalized * speed;


		movement = transform.TransformDirection (movement);

		movement.y = gravity;	
		movement *= Time.deltaTime;

		_charController.Move (movement);
	}
}
