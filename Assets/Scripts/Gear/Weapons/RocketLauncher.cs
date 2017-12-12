﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : ProjectileWeapon
{
    void Start()
    {
        timeToReload = 3;
        bulletsPerMag = 5;
        timeToShoot = .50f;
        bulletsInMag = bulletsPerMag;
        isTrace = false;
    }
}
