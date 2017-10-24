using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
    GameObject turret;
    GameObject placedTurret;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaceTurret(GameObject turretToPlace, TurretType type)
    {
        turret = turretToPlace;
        Vector3 turPosition = new Vector3(0,0.25f,0);
        placedTurret = Instantiate(turret, turPosition+transform.position, Quaternion.identity);
        Debug.Log("turrPos: " + turPosition + " placePos: " + placedTurret.transform.position);
        placedTurret.transform.parent = transform;
        placedTurret.AddComponent<BoxCollider>();
        BoxCollider turretCollider = placedTurret.GetComponent<BoxCollider>();
        turretCollider.center += new Vector3(0, 0.5f, 0);
        placedTurret.AddComponent<Turret>();
        Turret turretComponent = placedTurret.GetComponent<Turret>();
        turretComponent.SetupTurret(type);
        placedTurret.transform.tag = "Tower";
    }
    public void DestroyTurret()
    {
        GameObject.Destroy(placedTurret);
    }
    public bool HasTurret()
    {
        return placedTurret != null;
    }
}
public enum TurretType
{
    rifleTurret,
    rocketTurret,
    aoeTurret,
    cannonTurret
}
