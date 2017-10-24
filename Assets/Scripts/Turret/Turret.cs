using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public int cost;
    public TurretType turretType;
    public Equipment equipment;
    /*
    float damage;
    int cost;
    float attackRadius;
    float attackRange;
    float fireRate;
    */
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
    }
}
public enum TurretFocus
{
    first, last, strongest, weakest
}
