using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void reactToHit ()
	{
		StartCoroutine (Die ());
		FollowingEnemy behavior = GetComponent<FollowingEnemy> ();
		if (behavior != null)
			behavior.kill ();

		Destroy (this.gameObject);
	}

	private IEnumerator Die ()
	{
		FollowingEnemy behavior = GetComponent<FollowingEnemy> ();
		if (behavior != null)
			behavior.kill ();
		
		this.transform.Rotate (-90, 0, 0);

		yield return new WaitForSeconds (1.5f);

		Destroy (this.gameObject);
	}
}
