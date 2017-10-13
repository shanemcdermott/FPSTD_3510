using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

	public bool verticalAllowed = false;
	public bool horizontalAllowed = true;

	public float verticalSensitivity = 9f;
	public float HorizontalSensitivity = 9f;

	public float minVerticalRotation = -45f;
	public float maxVerticalRotation = 45f;

	private float rotationX = 0f;

	// Use this for initialization
	void Start ()
	{
		Rigidbody body = GetComponent<Rigidbody> ();
		if (body != null)
			body.freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (verticalAllowed && horizontalAllowed) {
			rotationX -= Input.GetAxis ("Mouse Y") * verticalSensitivity;
			rotationX = Mathf.Clamp (rotationX, minVerticalRotation, maxVerticalRotation);

			float delta = Input.GetAxis ("Mouse X") * HorizontalSensitivity;
			float rotationY = transform.localEulerAngles.y + delta;

			transform.localEulerAngles = new Vector3 (rotationX, rotationY, 0f);
	
		} else if (verticalAllowed) {
			rotationX -= Input.GetAxis ("Mouse Y") * verticalSensitivity;
			rotationX = Mathf.Clamp (rotationX, minVerticalRotation, maxVerticalRotation);

			float rotationY = transform.localEulerAngles.y;

			transform.localEulerAngles = new Vector3 (rotationX, rotationY, 0f);
		} else if (horizontalAllowed) {
			transform.Rotate (0, Input.GetAxis ("Mouse X") * HorizontalSensitivity, 0);
		}

	}
}