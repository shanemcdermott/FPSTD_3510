using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Equipment
{
    /*Amount of time that must pass before being able to shoot again */
    public float timeBetweenShots = 0.15f;
    /*Maximum Number of bullets in a magazine*/
    public int bulletsPerMag = 100;
    /*Amount of time it takes to replenish magazine*/
    public float timeToReload = 1.0f;

    protected float timer;
    protected bool bIsReloading;
    protected int bulletsInMag;


    void Awake()
    {
        timer = 0f;
        bulletsInMag = bulletsPerMag;
        bIsReloading = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    public override bool CanActivate()
    {
        return !IsBusy() && HasBullets() 
            && timer >= timeBetweenShots && Time.timeScale != 0;
    }

    public void StartReloading()
    {
        timer = 0f;
        bIsReloading = true;
    }

    public void StopReloading()
    {
        if(IsReloading() && timer >= timeToReload)
        {
            bulletsInMag = bulletsPerMag;
        }

        bIsReloading = false;
    }

    public bool HasBullets()
    {
        return bulletsInMag > 0;
    }

    public bool IsReloading()
    {
        return bIsReloading;
    }

    public override bool IsBusy()
    {
        return base.IsBusy() || bIsReloading;
    }
}
