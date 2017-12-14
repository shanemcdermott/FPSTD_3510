using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IFocusable
{
    

    public Equipment equipment;
    public TurretType turretType;
    public int RifleDamage = 100;
    public int RocketDamage = 400;
    public int CannonDamage = 200;
    int cost;
    float attackRadius;
    public float attackRange;
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



    // Use this for initialization
    void Start()
    {
		
    }

	void Awake()
	{
		mainTransform = null;
		headTransform = null;
		xTargRot = 0f;
		yTargRot = 0f;
		xCurrRot = 0f;
		yCurrRot = 0f;

        Debug.Log("turrType: " + turretType);

	}

	public void setFocus(TurretFocus foc)
	{
		focusType = foc;
	}

    // Update is called once per frame
    void Update()
    {
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


        switch (turretType)
        {
            case TurretType.rifleTurret:
                this.GetComponent<Rifle>().damagePerShot = RifleDamage;
                break;
            case TurretType.rocketTurret:
                this.GetComponent<RocketLauncher>().damage = RocketDamage;
                break;
            case TurretType.cannonTurret:
                this.GetComponent<Sniper>().damagePerShot = CannonDamage;
                break;
        }


		//shoot at target
		if (equipment != null && equipment.CanActivate() && target != null)
            equipment.Activate();
			

	}

	public void selectTarget()
	{
		GameObject [] gos = GameObject.FindGameObjectsWithTag("Enemy");
		target = null;

		if (focusType == TurretFocus.first)
		{
			float shortestPath = float.MaxValue;

			foreach (GameObject go in gos)
			{
				Vector3 temp = this.transform.position - go.transform.position;
				float dist = temp.x * temp.x + temp.z * temp.z;

				if (dist > attackRange * attackRange)
					continue;

				if (go.GetComponent<EnemyHealth>().currentHealth == 0)
					continue;
				
				float pathlen = go.GetComponent<MonsterController>().getPathLength();
				if (pathlen < shortestPath)
				{
					target = go;
					shortestPath = pathlen;
				}

			}
		}
		else if (focusType == TurretFocus.last)
		{
			float longestPath = 0;

			foreach (GameObject go in gos)
			{
				Vector3 temp = this.transform.position - go.transform.position;
				float dist = temp.x * temp.x + temp.z * temp.z;

				if (dist > attackRange * attackRange)
					continue;

				if (go.GetComponent<EnemyHealth>().currentHealth == 0)
					continue;

				float pathlen = go.GetComponent<MonsterController>().getPathLength();
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
				Vector3 temp = this.transform.position - go.transform.position;
				float dist = temp.x * temp.x + temp.z * temp.z;

				if (dist > attackRange * attackRange)
					continue;

				if (go.GetComponent<EnemyHealth>().currentHealth == 0)
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
				Vector3 temp = this.transform.position - go.transform.position;
				float dist = temp.x * temp.x + temp.z * temp.z;

				if (dist > attackRange * attackRange)
					continue;

				if (go.GetComponent<EnemyHealth>().currentHealth == 0)
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
				float dist = temp.x * temp.x + temp.z * temp.z;

				if (dist > attackRange * attackRange)
					continue;

				if (go.GetComponent<EnemyHealth>().currentHealth == 0)
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

    public void SetupTurret(AudioSource source, ParticleSystem ps)
    {
        focusType = TurretFocus.first;
        
		setupTransforms(); //set main and head transforms for proper rotation

        switch (turretType)
        {
            case TurretType.rifleTurret:
                cost = 10;
                attackRadius = 0;
                attackRange = 30;
                fireRate = 0.15f;
                //damage = 100;

				this.gameObject.AddComponent<Rifle>();
				Rifle r = this.gameObject.GetComponent<Rifle>();
                
                r.gunAudio = source;
                r.gunParticles = ps;
                r.usesAmmo = false;
                r.state = WeaponState.Idle;
                r.DisableEffects();

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
                //damage = 400;

                this.gameObject.AddComponent<RocketLauncher>();
                RocketLauncher rl = this.gameObject.GetComponent<RocketLauncher>();

                rl.gunAudio = source;
                rl.gunParticles = ps;
                rl.usesAmmo = false;
                rl.state = WeaponState.Idle;

                if (headTransform != null)
                    rl.aimTransform = headTransform;
                else
                    rl.aimTransform = this.transform;

                equipment = rl;

                break;
            case TurretType.cannonTurret:
                cost = 10;
                attackRadius = 0;
                attackRange = 250;
                fireRate = 0.5f;
                //damage = 200;

                this.gameObject.AddComponent<Sniper>();
                Sniper s = this.gameObject.GetComponent<Sniper>();

                s.gunAudio = source;
                s.gunParticles = ps;
                s.usesAmmo = false;
                s.state = WeaponState.Idle;

                if (headTransform != null)
                    s.aimTransform = headTransform;
                else
                    s.aimTransform = this.transform;

                equipment = s;
                break;
            case TurretType.aoeTurret:
                cost = 10;
                attackRadius = 10;
                attackRange = 0;
                fireRate = 0.5f;
                //damage = 500;

                this.gameObject.AddComponent<Pulse>();
                Pulse p = this.gameObject.GetComponent<Pulse>();

                p.gunAudio = source;
                p.gunParticles = ps;
                p.usesAmmo = false;
                p.state = WeaponState.Idle;

                if (headTransform != null)
                    p.aimTransform = headTransform;
                else
                    p.aimTransform = this.transform;

                equipment = p;

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

	public void rotateFocus()
	{
		if (focusType = TurretFocus.closest)
		{
			focusType = TurretFocus.first;
		}
		else if (focusType = TurretFocus.first)
		{
			focusType = TurretFocus.last;
		}
		else if (focusType = TurretFocus.last)
		{
			focusType = TurretFocus.strongest;
		}
		else if (focusType = TurretFocus.strongest)
		{
			focusType = TurretFocus.weakest;
		}
		else if (focusType = TurretFocus.weakest)
		{
			focusType = TurretFocus.closest;
		}
		Debug.Log("Turret set to target " + focusType);
	}
}

public enum TurretFocus
{
    first, last, strongest, weakest, closest
}
