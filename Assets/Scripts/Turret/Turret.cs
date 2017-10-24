using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
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
}
public enum TurretFocus
{
    first, last, strongest, weakest
}
