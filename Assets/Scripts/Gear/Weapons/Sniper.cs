using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : TraceWeapon
{

    public Camera fpsCam;
    public bool isZoomed;

	// Use this for initialization
    
	void Start () {
        isZoomed = false;
       /* damage = 1000;
        range = 1000;
        timeToReload = 2;
        magazineCapacity = 5;
        fireRate = .75f;
        bIsReloading = false;
        bIsBusy = false;
        bulletsInMag = magazineCapacity;
        timer = 0f;
        */
    }
	
	// Update is called once per frame

    public override void Activate()
    {
        if(isZoomed)
        {
            ZoomedShoot();
        }
        else
        {
            base.Activate();
        }
    }

    public void ZoomedShoot()
    {
        shootRay.origin = fpsCam.transform.position;
        shootRay.direction = fpsCam.transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            Debug.Log("Object Hit: " + shootHit.transform.name);
            HealthComponent enemyHealth = shootHit.collider.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(new DamageContext(gameObject, damagePerShot, shootHit.point));
            }
            traceLine.SetPosition(1, shootHit.point);
        }
        else
        {
            traceLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    public override void StartReloading()
    {
        base.StartReloading();
        isZoomed = false;
    }
}
