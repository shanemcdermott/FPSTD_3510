using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A weapon that performs a raytrace for hit detection */

public class TraceWeapon : Weapon
{

    public int damagePerShot = 20;
    public float range = 100f;


    protected Ray shootRay = new Ray();
    protected RaycastHit shootHit;
    int shootableMask;
    LineRenderer traceLine;

    ParticleSystem gunParticles;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    protected override void Awake()
    {
        base.Awake();
        shootableMask = LayerMask.GetMask("Shootable");
        traceLine = GetComponent<LineRenderer>();

        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    protected override void Update()
    {
        base.Update();
        if (timer >= timeBetweenShots * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public override void Activate()
    {
        if(CanActivate())
        {
            timer = 0f;

            gunAudio.Play();

            gunLight.enabled = true;

            gunParticles.Stop();
            gunParticles.Play();
            traceLine.enabled = true;
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
            EnableEffects();
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
        gunLight.enabled = false;
        //
    }

    public override void EnableEffects()
    {
        //traceLine.enabled = true;
        //throw new NotImplementedException();
    }

}
