using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyMovement : MonoBehaviour, IRespondsToDeath
{
	
    public float speed;
	Vector3 targetLocation;


    void Awake ()
    {
        GetComponent<HealthComponent>().RegisterDeathResponder(this);
		targetLocation = this.transform.position;
    }

	public void setTargetPosition(Vector3 position)
	{
		targetLocation = position;
	}


    void FixedUpdate ()
	{
		if (enabled)
		{
			float zDiff = targetLocation.z - this.transform.position.z;
			float xDiff = targetLocation.x - this.transform.position.x;
	        this.transform.localEulerAngles = new Vector3(0, (Mathf.Atan2(xDiff, zDiff) / Mathf.PI * 180), 0);
	        this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
			GetComponent<Animator>().SetBool("isWalking", true);
		}
    }


    public void OnDeath(DamageContext context)
    {
        enabled = false;
		GetComponent<Animator>().SetBool("isWalking", false);
    }

}
