using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public Projectile projectileToLaunch;
    public float launchSpeed = 50;

    public override void Activate()
    {
        //if (!CanActivate()) return;

        SetCurrentState(WeaponState.HipFiring);
        if (usesAmmo)
            bulletsInMag--;

        EnableEffects();

        Transform t = transform;
        if (useRootTransform)
        {
            t = transform.root;
        }
            //This works for the player but not the game object...
            Vector3 origin = t.position + t.TransformDirection(Vector3.forward * 2);
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
