using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon {
    // Use this for initialization
    void Start () {
        damage = 100;
        range = 300;
        timeToReload = 1;
        magazineCapacity = 30;
        fireRate = .15f;
        bIsReloading = false;
        bIsBusy = false;
        bulletsInMag = magazineCapacity;
        timer = 0f;
    }

    // Update is called once per frame
    void Update () {
		
	}
    public override void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Object Hit: " + hit.transform.name);
            MonsterController enemy = hit.transform.GetComponent<MonsterController>();
            enemy.TakeDamage(damage);
        }
        //TraceWeapon trace = this.GetComponent<TraceWeapon>();
        //trace.Activate();
    }
    public override void Activate()
    {

    }
    public override void Deactivate()
    {

    }

    public override void EnableEffects()
    {

    }
    public override void DisableEffects()
    {

    }


    public override bool CanActivate()
    {
        return !IsBusy() && HasBullets()
            && timer >= fireRate && Time.timeScale != 0;
    }

    public override void StartReloading()
    {
        timer = 0f;
        bIsReloading = true;
    }

    public override void StopReloading()
    {
        if (IsReloading() && timer >= timeToReload)
        {
            bulletsInMag = magazineCapacity;
        }

        bIsReloading = false;
    }

    public override bool HasBullets()
    {
        return bulletsInMag > 0;
    }

    public override bool IsReloading()
    {
        return bIsReloading;
    }
}
