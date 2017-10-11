using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
    GameObject turret;
    bool hasTurret = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaceTurret(GameObject turretToPlace, TurretType type)
    {
        turret = turretToPlace;
        turret.transform.position = gameObject.transform.position + new Vector3(0,1.5f,0);
        gameObject.AddComponent<Turret>();
        Turret turretComponent = gameObject.GetComponent<Turret>();
        turretComponent.SetupTurret(type);
        Transform transform = Instantiate(turret.transform) as Transform;
        transform.parent = this.transform;
        hasTurret = true;
    }
    public bool HasTurret()
    {
        return hasTurret;
    }
}
public enum TurretType
{
    rifleTurret,
    grenadeTurret,
    aoeTurret,
    cannonTurret
}
