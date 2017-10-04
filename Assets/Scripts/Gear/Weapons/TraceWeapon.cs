using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A weapon that performs a raytrace for hit detection */

public class TraceWeapon : Weapon
{

    public int damagePerShot = 20;
    public float range = 100f;
    public float effectsDisplayTime = 0.2f;
    public LineRenderer traceLine;
    public ParticleSystem gunParticles;
    public AudioSource gunAudio;
    public Light gunLight;

    protected Ray shootRay = new Ray();
    protected RaycastHit shootHit;
    int shootableMask;


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

    protected override void Update()
    {
        base.Update();
        if (timer >= effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public override void Activate()
    {
        if(CanActivate())
        {
            if (usesAmmo)
                bulletsInMag--;

            timer = 0f;

            EnableEffects();
            traceLine.SetPosition(0, transform.position);

            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            
            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
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
        else if(IsReloading() && timer >= timeToReload)
        {
            StopReloading();
        }
        else if(!HasBullets() && !IsReloading())
        {
            StartReloading();
        }
    }

    public override void Deactivate()
    {
        //
        SetIsBusy(false);
        DisableEffects();
    }

    public override void DisableEffects()
    {
        traceLine.enabled = false;
        if(gunLight != null)
            gunLight.enabled = false;
        if (gunAudio != null)
            gunAudio.Stop();
        if (gunParticles != null)
        {
            gunParticles.Stop();
        }
    }

    public override void EnableEffects()
    {
        if(gunAudio != null)
            gunAudio.Play();

        if(gunLight != null)
            gunLight.enabled = true;

        if (gunParticles != null)
        {
            gunParticles.Play();
        }

        traceLine.enabled = true;
    }
}
