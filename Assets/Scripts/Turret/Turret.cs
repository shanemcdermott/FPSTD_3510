using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IFocusable
{

    public Equipment equipment;
    public TurretType turretType; //TODO: is this the issue?

    float damage;
    int cost;
    float attackRadius;
    float attackRange;
    float fireRate;
    TurretFocus focusType;

	Transform mainTransform;//yAngle controls main transform
	Transform headTransform; //xAngle controls head transform

	float xCurrRot;
	float yCurrRot;
	float xTargRot;
	float yTargRot;

	float rotationSpeed = 120f;

	GameObject target;


	void Awake()
	{
		mainTransform = null;
		headTransform = null;
		xTargRot = 0f;
		yTargRot = 0f;
		xCurrRot = 0f;
		yCurrRot = 0f;
	}

	public void setFocus(TurretFocus foc)
	{
		focusType = foc;
	}
		
	void FixedUpdate()
	{

		selectTarget();

		//aim turret
		if (target != null)
		{
			//find the target rotation values
			Vector3 pos_diff = target.transform.position - this.transform.position;
			xTargRot = (Mathf.Atan2 (Mathf.Abs(pos_diff.y), Mathf.Sqrt(pos_diff.x * pos_diff.x + pos_diff.z * pos_diff.z)) / Mathf.PI * 180);
			yTargRot = (Mathf.Atan2 (pos_diff.x, pos_diff.z) / Mathf.PI * 180);

		}
		else
		{
			xTargRot = 0;
		}

		//calculate how much the turret can move based on rotation speed
		float x_rot_delta = xTargRot - xCurrRot > 180 ? xTargRot - xCurrRot - 360 : xTargRot - xCurrRot;
		float y_rot_delta = yTargRot - yCurrRot > 180 ? yTargRot - yCurrRot - 360 : yTargRot - yCurrRot;
		float speed = target == null ? rotationSpeed / 4 : rotationSpeed; //move slower if no target
		x_rot_delta = Mathf.Clamp(x_rot_delta, rotationSpeed * Time.deltaTime * -1, speed * Time.deltaTime);
		y_rot_delta = Mathf.Clamp(y_rot_delta, rotationSpeed * Time.deltaTime * -1, speed * Time.deltaTime);

		//update the current rotation values and rotate the transforms
		xCurrRot += x_rot_delta;
		yCurrRot += y_rot_delta;					
		if (mainTransform != null)
			mainTransform.localEulerAngles = new Vector3 (0, yCurrRot, 0);
		if (headTransform != null)
			headTransform.transform.localEulerAngles = new Vector3(xCurrRot, 0, 0);
		


		//TODO: better shooting at target
		if (equipment != null)
			equipment.Activate();

	}

	public void selectTarget()
	{
		GameObject [] gos = GameObject.FindGameObjectsWithTag("Enemy");
		target = null;

		if (focusType == TurretFocus.first)
		{
			int shortestPath = int.MaxValue;

			foreach (GameObject go in gos)
			{
				if (!goodCandidate(go))
					continue;
				
				int pathlen = go.GetComponent<EnemyMovement>().getPathLen();
				if (pathlen < shortestPath)
				{
					target = go;
					shortestPath = pathlen;
				}

			}
		}
		else if (focusType == TurretFocus.last)
		{
			int longestPath = 0;

			foreach (GameObject go in gos)
			{
				if (!goodCandidate(go))
					continue;

				int pathlen = go.GetComponent<EnemyMovement>().getPathLen();
				if (pathlen > longestPath)
				{
					target = go;
					longestPath = pathlen;
				}

			}
		}
		else if (focusType == TurretFocus.strongest)
		{
			int strongest = 0;
			foreach (GameObject go in gos)
			{
				if (!goodCandidate(go))
					continue;

				int temphealth = go.GetComponent<EnemyHealth>().currentHealth;
				if (temphealth > strongest)
				{
					target = go;
					strongest = temphealth;
				}
			}

		}
		else if (focusType == TurretFocus.weakest)
		{
			int weakest = int.MaxValue;
			foreach (GameObject go in gos)
			{
				if (!goodCandidate(go))
					continue;

				int temphealth = go.GetComponent<EnemyHealth>().currentHealth;
				if (temphealth < weakest)
				{
					target = go;
					weakest = temphealth;
				}
			}
		}
		else if (focusType == TurretFocus.closest)
		{
			float closestdist = 0f;
			foreach (GameObject go in gos)
			{
				Vector3 temp = this.transform.position - go.transform.position;

				if (!goodCandidate(go))
					continue;

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

			}
		}

	}

	private bool goodCandidate(GameObject go)
	{
		Vector3 temp = this.transform.position - go.transform.position;
		float dist = temp.x * temp.x + temp.z * temp.z;

		if (dist > attackRange * attackRange)
			return false;

		if (go.GetComponent<EnemyHealth>().currentHealth == 0)
			return false;

		return true;
	}

    public void SetupTurret(TurretType turrType)
    {
        focusType = TurretFocus.first;

		turretType = turrType; //set the turret type
		setupTransforms(); //set main and head transforms for proper rotation

        switch (turretType)
        {
            case TurretType.rifleTurret:
                cost = 10;
                attackRadius = 0;
                attackRange = 30;
                fireRate = 0.15f;
                damage = 100;

				this.gameObject.AddComponent<Rifle>();
				Rifle r = this.gameObject.GetComponent<Rifle>();
		
				if (headTransform != null)
					r.aimTransform = headTransform;
				else
					r.aimTransform = this.transform;

				equipment = r;

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

	//correctly assigns the main and head transforms to turrets for realistic rotation
	void setupTransforms()
	{
		if (turretType != TurretType.aoeTurret)
			mainTransform = this.transform;

		if (turretType == TurretType.aoeTurret)
		{
			headTransform = null;
		}
		else if (turretType == TurretType.cannonTurret)
		{
			Transform [] transforms = this.GetComponentsInChildren<Transform>();
			foreach (Transform h in transforms)
				if (h.name.Equals("Head001"))
					headTransform = h;
		}
		else
		{
			Transform [] transforms = this.GetComponentsInChildren<Transform>();
			foreach (Transform h in transforms)
				if (h.name.Equals("Head"))
					headTransform = h;
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
    first, last, strongest, weakest, closest
}
