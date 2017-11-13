using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject projectileToLaunch;
    public float launchSpeed = 50;
    public float fireOffset = 2;
    

    public override void Activate()
    {
        if (!CanActivate()) return;
        shootTimer = 0.0f;
        SetCurrentState(WeaponState.HipFiring);
        if (usesAmmo)
            bulletsInMag--;

        EnableEffects();

        //Needs to use camera rotation!
        Vector3 origin = gameObject.transform.position + new Vector3(0.25f, 0, 0.5f);
        Quaternion rot = mainCamera.transform.rotation;
        GameObject proj = Instantiate(projectileToLaunch, origin, rot);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.SetCreator(transform.root.gameObject);
        Rigidbody rigidBody = proj.GetComponent<Rigidbody>();
        rigidBody.velocity = mainCamera.transform.TransformVector(Vector3.forward * launchSpeed);
    }

    public override void Deactivate()
    {
        //SetCurrentState(WeaponState.Idle);
        DisableEffects();
    }
	
}
