using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IFocusable
{
    public Equipment equipment;
    public TurretType turretType;
    //public Equipment equipment;
    float damage;
    int cost;
    float attackRadius;
    float attackRange;
    float fireRate;
    TurretFocus focusType;




    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

	void FixedUpdate()
	{
		GameObject [] gos = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject target = null;
		float closestdist = 0f;
		foreach (GameObject go in gos)
		{
			Vector3 temp = this.transform.position - go.transform.position;
			if (target == null)
			{
				target = go;
				closestdist = temp.x * temp.x + temp.z * temp.z;
			}
			else
			{
				float candidatedist = temp.x * temp.x + temp.z * temp.z;
				if (candidatedist < closestdist)
				{
					closestdist = candidatedist;
					target = go;
				}
			}
				

			float zDiff = target.transform.position.z - this.transform.position.z;
			float yDiff = target.transform.position.y - this.transform.position.y;
			float xDiff = target.transform.position.x - this.transform.position.x;
			float xAngle = (Mathf.Atan2 (Mathf.Abs(yDiff), Mathf.Sqrt(xDiff * xDiff + zDiff * zDiff)) / Mathf.PI * 180);
			float yAngle = (Mathf.Atan2 (xDiff, zDiff) / Mathf.PI * 180);

			this.transform.localEulerAngles = new Vector3 (0, yAngle, 0);

			Transform [] transforms = this.GetComponentsInChildren<Transform>();
			foreach (Transform head in transforms)
				if (head.name.Equals("Head"))
					head.transform.localEulerAngles = new Vector3(xAngle, 0, 0);

			//TODO: shoot at target
		}

	}

    public void SetupTurret(TurretType turrType)
    {
        focusType = TurretFocus.first;
        switch (turretType)
        {
            case TurretType.rifleTurret:
                cost = 10;
                attackRadius = 0;
                attackRange = 100;
                fireRate = 0.15f;
                damage = 100;
                break;
            case TurretType.rocketTurret:
                cost = 10;
                attackRadius = 50;
                attackRange = 200;
                fireRate = 0.75f;
                damage = 400;
                break;
            case TurretType.cannonTurret:
                cost = 10;
                attackRadius = 0;
                attackRange = 250;
                fireRate = 0.5f;
                damage = 200;
                break;
            case TurretType.aoeTurret:
                cost = 10;
                attackRadius = 10;
                attackRange = 0;
                fireRate = 0.5f;
                damage = 500;
                break;
        }
    }
    public void onBeginFocus(PlayerController focuser)
    {
        //pop up the turret upgrade menu
    }
    public void onEndFocus(PlayerController focuser)
    {
        //hide the turret upgrade menu
    }
}

public enum TurretFocus
{
    first, last, strongest, weakest
}
