using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A weapon that performs a raytrace for hit detection */

public class TraceWeapon : Weapon
{
    public int damagePerShot = 20;
    public float range = 100f;
    public LineRenderer traceLine;

    protected Ray shootRay = new Ray();
    protected RaycastHit shootHit;
    protected int shootableMask;


    protected override void Awake()
    {
        base.Awake();
        shootableMask = LayerMask.GetMask("Shootable");
        if(traceLine == null)
            traceLine = GetComponent<LineRenderer>();
        if(gunParticles == null)
            gunParticles = GetComponent<ParticleSystem>();
        if(gunAudio == null)
            gunAudio = GetComponent<AudioSource>();
        if(gunLight == null)
            gunLight = GetComponent<Light>();
    }

    public override void Activate()
    {
        //if (!CanActivate()) return;

        SetCurrentState(WeaponState.HipFiring);

        if (usesAmmo)
            bulletsInMag--;

        timer = 0f;

        EnableEffects();
        traceLine.SetPosition(0, transform.position);

        if (useRootTransform)
        {
            shootRay.origin = transform.root.position;
            shootRay.direction = transform.root.forward;
        }
        else
        {
            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;
        }
        //transform.root works for player but not for towers...

            
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            HealthComponent enemyHealth = shootHit.collider.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(new DamageContext(transform.root.gameObject, damagePerShot, shootHit.point));
            }
            traceLine.SetPosition(1, shootHit.point);
        }
        else
        {
            traceLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
        
    }

    public override void Deactivate()
    {
        //SetCurrentState(WeaponState.Idle);
        DisableEffects();
    }

    public override void DisableEffects()
    {
        base.DisableEffects();
        traceLine.enabled = false;
    }

    public override void EnableEffects()
    {
        base.EnableEffects();
        traceLine.enabled = true;
    }

}
