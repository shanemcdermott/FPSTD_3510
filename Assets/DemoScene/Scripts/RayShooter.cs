using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{

	private Camera _camera;

	// Use this for initialization
	void Start ()
	{
		_camera = GetComponent<Camera> ();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void OnGUI ()
	{
		int size = 12;
		float posx = _camera.pixelWidth / 2 - size / 4;
		float posy = _camera.pixelHeight / 2 - size / 2;
		GUI.Label (new Rect (posx, posy, size, size), "*");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButton("Fire1")) {
			Vector3 point = new Vector3 (_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
			Ray ray = _camera.ScreenPointToRay (point);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				GameObject hitObject = hit.transform.gameObject;
				HealthComponent target = hitObject.GetComponent<HealthComponent> ();
				if (target != null) {
                    target.TakeDamage(new DamageContext(gameObject, 1));
					//target.reactToHit ();
					Debug.Log ("Hit Target");
				} else {
					//StartCoroutine (SphereIndicator (hit.point));
				}
			}
		}
	}

	private IEnumerator SphereIndicator (Vector3 pos)
	{
		GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		sphere.transform.position = pos;

		yield return new WaitForSeconds (1);

		Destroy (sphere);
	}
}
