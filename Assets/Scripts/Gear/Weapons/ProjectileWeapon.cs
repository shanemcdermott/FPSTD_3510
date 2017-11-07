using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public Projectile projectileToLaunch;
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
        Vector3 origin = aimTransform.position + aimTransform.TransformDirection(Vector3.forward * fireOffset);
        Quaternion rot = aimTransform.rotation;
        Projectile proj = Instantiate(projectileToLaunch, origin, rot);
        proj.SetCreator(transform.root.gameObject);
        Rigidbody rigidBody = proj.GetComponent<Rigidbody>();
        rigidBody.velocity = aimTransform.TransformDirection(Vector3.forward * launchSpeed);
    }

    public override void Deactivate()
    {
        //SetCurrentState(WeaponState.Idle);
        DisableEffects();
    }
	
}
