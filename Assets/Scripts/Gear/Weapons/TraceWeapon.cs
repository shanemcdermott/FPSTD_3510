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

	GameObject go;


    protected override void Awake()
    {
        base.Awake();
        if(gunParticles == null)
            gunParticles = GetComponent<ParticleSystem>();
        if(gunAudio == null)
            gunAudio = GetComponent<AudioSource>();
        if(gunLight == null)
            gunLight = GetComponent<Light>();

		go = GameObject.CreatePrimitive(PrimitiveType.Sphere);


    }
		

    public override void Activate()
    {
		GameObject.Destroy(go);

        SetCurrentState(WeaponState.HipFiring);

        if (usesAmmo)
            bulletsInMag--;

        shootTimer = 0f;

        EnableEffects();

        shootRay.origin = aimTransform.position;
        shootRay.direction = aimTransform.forward;

            
		if (Physics.Raycast(shootRay, out shootHit, range))
		{

			HealthComponent enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
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
