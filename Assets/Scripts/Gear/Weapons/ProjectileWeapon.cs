using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public Projectile projectileToLaunch;
    public float launchSpeed;

    public override void Activate()
    {
        SetCurrentState(WeaponState.HipFiring);
        if (usesAmmo)
            bulletsInMag--;

        EnableEffects();

        GameObject p = GameManager.instance.GetPlayer().gameObject;
        Vector3 origin = p.transform.position + p.transform.TransformDirection(Vector3.forward * 2);
        Quaternion rot = p.transform.rotation;
        Projectile proj = Instantiate(projectileToLaunch, origin, rot);
        proj.SetCreator(p);
        Rigidbody rigidBody = proj.GetComponent<Rigidbody>();
        rigidBody.velocity = p.transform.TransformDirection(Vector3.forward * launchSpeed);
    }

    public override void Deactivate()
    {
        SetCurrentState(WeaponState.Idle);
        DisableEffects();
    }
	
}
