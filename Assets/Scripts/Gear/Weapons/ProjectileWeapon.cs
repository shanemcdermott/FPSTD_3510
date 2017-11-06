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
        timer = 0.0f;
        SetCurrentState(WeaponState.HipFiring);
        if (usesAmmo)
            bulletsInMag--;

        EnableEffects();

        Transform t = transform;
        if (useRootTransform)
        {
            t = transform.root;
        }
            //Needs to use camera rotation!
            Vector3 origin = t.position + t.TransformDirection(Vector3.forward * fireOffset);
            Quaternion rot = t.rotation;
            Projectile proj = Instantiate(projectileToLaunch, origin, rot);
            proj.SetCreator(transform.root.gameObject);
            Rigidbody rigidBody = proj.GetComponent<Rigidbody>();
            rigidBody.velocity = t.TransformDirection(Vector3.forward * launchSpeed);
    }

    public override void Deactivate()
    {
        //SetCurrentState(WeaponState.Idle);
        DisableEffects();
    }
	
}
