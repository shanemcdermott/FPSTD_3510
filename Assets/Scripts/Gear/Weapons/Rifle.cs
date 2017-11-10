using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : TraceWeapon {

    // Use this for initialization
    void Start() {
        damagePerShot = 100;
        range = 500;
        timeToReload = 1;
        bulletsPerMag = 30;
        timeToShoot = 0.1f;
        bulletsInMag = bulletsPerMag;
    }
}
