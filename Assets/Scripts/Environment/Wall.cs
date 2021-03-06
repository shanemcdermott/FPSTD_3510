﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IFocusable {
    GameObject turret;
    GameObject placedTurret;


    public ParticleSystem muzzleFlash;

    public AudioSource rifleAudio;
    public AudioSource cannonAudio;
    public AudioSource rocketAudio;
    public AudioSource pulseAudio;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onBeginFocus(PlayerController focuser)
    {
        GameObject turretToShow = focuser.transparentStuff[(int)focuser.currentTurretType + 1];
        if (transform.parent.GetComponent<Tile>().HasWall())
        {
            if (placedTurret == null)
            {
                turretToShow.transform.position = transform.position + new Vector3(0, 0.25f, 0);
                turretToShow.SetActive(true);
            }
        }
    }
    public void onEndFocus(PlayerController focuser)
    {
        GameObject turretToHide = focuser.transparentStuff[(int)focuser.currentTurretType + 1];
        turretToHide.SetActive(false);
    }

    public void PlaceTurret(GameObject turretToPlace, TurretType type, int damageMod, float attackRange)
    {
        turret = turretToPlace;
		//this.transform.lossyScale.y * 0.25
		Vector3 turPosition = new Vector3(0,this.transform.lossyScale.y * 0.5f,0);

        placedTurret = Instantiate(turret, turPosition+transform.position, Quaternion.identity);
        placedTurret.transform.parent = transform;
		placedTurret.transform.localScale = new Vector3(1, 1, 1);
        placedTurret.AddComponent<BoxCollider>();
        
        BoxCollider turretCollider = placedTurret.GetComponent<BoxCollider>();
        turretCollider.center += new Vector3(0, 0.5f, 0);

        placedTurret.AddComponent<Turret>();
        Turret turretComponent = placedTurret.GetComponent<Turret>();

        turretComponent.turretType = type;
        turretComponent.RifleDamage += damageMod;
        turretComponent.RocketDamage += damageMod;
        turretComponent.CannonDamage += damageMod;
        
        switch (type)
        {
            case TurretType.rifleTurret:
                turretComponent.SetupTurret(rifleAudio, muzzleFlash);
                break;
            case TurretType.cannonTurret:
                turretComponent.SetupTurret(cannonAudio, muzzleFlash);
                break;
            case TurretType.rocketTurret:
                turretComponent.SetupTurret(rocketAudio, muzzleFlash);
                break;
            case TurretType.aoeTurret:
                turretComponent.SetupTurret(pulseAudio, muzzleFlash);
                break;
        }
        turretComponent.attackRange += attackRange;
        
        
        placedTurret.transform.tag = "Tower";
        placedTurret.SetActive(true);
    }
    public void DestroyTurret()
    {
        GameObject.Destroy(placedTurret);
        placedTurret = null;
    }
    public bool HasTurret()
    {
        return placedTurret != null;
    }
}
public enum TurretType
{
    rifleTurret,
    cannonTurret,
    rocketTurret,
    aoeTurret
}
