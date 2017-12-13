using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject projectileToLaunch;
    public GameObject rocketSpawner;
    public float launchSpeed = 50;
    public float fireOffset = 2;
    public int damage = 20;
    public float radius = 10f;
    public float power = 1000f;
    public float lift = 30f;
    
    public override void Activate()
    {
        if (!CanActivate()) return;
        shootTimer = 0.0f;
        SetCurrentState(WeaponState.HipFiring);
        if (usesAmmo)
            bulletsInMag--;

        EnableEffects();

        Vector3 origin = rocketSpawner.transform.position;
        Quaternion rot = rocketSpawner.transform.rotation;
        GameObject proj = Instantiate(projectileToLaunch, origin, rot);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.damage = damage;
        projectile.radius = radius;
        projectile.power = power;
        projectile.lift = lift;
        projectile.SetCreator(transform.root.gameObject);
        Rigidbody rigidBody = proj.GetComponent<Rigidbody>();
        Vector3 velocity = rocketSpawner.transform.TransformVector(Vector3.forward * launchSpeed);
        rigidBody.velocity = velocity;
        Debug.Log("v: " + velocity + " rgv: " + rigidBody.velocity);
    }

    public override void Deactivate()
    {
        //SetCurrentState(WeaponState.Idle);
        DisableEffects();
    }
	
}
