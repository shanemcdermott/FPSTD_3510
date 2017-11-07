using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A weapon that performs a raytrace for hit detection */

public class TraceWeapon : Weapon
{
    public int damagePerShot = 20;
    public float range = 10000f;

    protected Ray shootRay = new Ray();
    protected RaycastHit shootHit;
    protected int shootableMask;


    protected override void Awake()
    {
        base.Awake();
        shootableMask = LayerMask.GetMask("Shootable");
        if(gunParticles == null)
            gunParticles = GetComponent<ParticleSystem>();
        if(gunAudio == null)
            gunAudio = GetComponent<AudioSource>();
        if(gunLight == null)
            gunLight = GetComponent<Light>();
    }

    public override void Activate()
    {
        Debug.Log("traceActivate");
        
        //if (!CanActivate()) return;

        SetCurrentState(WeaponState.HipFiring);

        if (usesAmmo)
            bulletsInMag--;

        shootTimer = 0f;

        EnableEffects();

        if (useRootTransform)
        {
            shootRay.origin = transform.root.position;
            shootRay.direction = transform.root.forward;
        }
        else
        {
            shootRay.origin = mainCamera.transform.position;
            shootRay.direction = mainCamera.transform.forward;
        }
        //transform.root works for player but not for towers...

            
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            Debug.Log("traceHitTag" + shootHit.transform.tag);
            HealthComponent enemyHealth = shootHit.collider.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(new DamageContext(transform.root.gameObject, damagePerShot, shootHit.point));
            }
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
    }

    public override void EnableEffects()
    {
        base.EnableEffects();
    }

}
