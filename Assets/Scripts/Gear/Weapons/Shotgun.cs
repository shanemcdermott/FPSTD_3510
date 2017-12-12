using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : TraceWeapon {

	// Use this for initialization
	void Start() {
        damagePerShot = 500;
        range = 250;
        timeToReload = 2;
        bulletsPerMag = 8;
        timeToShoot = 0.7f;
        bulletsInMag = bulletsPerMag;
        isTrace = true;
    }
}
