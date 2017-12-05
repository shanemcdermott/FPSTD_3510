using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IFocusable {
    GameObject turret;
    GameObject placedTurret;

    public Weapon rifle;
    public Weapon cannon;
    public Weapon rocket;
    public Weapon aoePulse;
    public Camera aimTransform;
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

    public void PlaceTurret(GameObject turretToPlace, TurretType type)
    {
        turret = turretToPlace;
        Vector3 turPosition = new Vector3(0,0.25f,0);
        
        placedTurret = Instantiate(turret, turPosition+transform.position, Quaternion.identity);
        placedTurret.SetActive(false);
        placedTurret.transform.parent = transform;
        placedTurret.AddComponent<BoxCollider>();

        placedTurret.AddComponent<TowerAim>();
        placedTurret.AddComponent<TowerAttackAI>();
        TowerAttackAI towerAtkAI = placedTurret.GetComponent<TowerAttackAI>();
        SetTowerAttackAI(towerAtkAI, type);

        BoxCollider turretCollider = placedTurret.GetComponent<BoxCollider>();
        turretCollider.center += new Vector3(0, 0.5f, 0);

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
    private void SetTowerAttackAI(TowerAttackAI tai, TurretType turType) {
        switch (turType)
        {
            case TurretType.rifleTurret:
                tai.equipment = rifle;
                break;
            case TurretType.cannonTurret:
                tai.equipment = cannon;
                break;
            case TurretType.rocketTurret:
                tai.equipment = rocket;
                break;
            case TurretType.aoeTurret:
                tai.equipment = aoePulse;
                break;
        }
        tai.equipment.usesAmmo = false;
        tai.equipment.aimTransform = placedTurret.transform;
    }
}
public enum TurretType
{
    rifleTurret,
    cannonTurret,
    rocketTurret,
    aoeTurret
}
