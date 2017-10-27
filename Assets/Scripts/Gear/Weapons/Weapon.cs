using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, Equipment
{
    /*Maximum Number of bullets in a magazine*/
    public int bulletsPerMag = 100;

    /*Amount of time it takes to fire one shot.*/
    public float timeToShoot = 0.15f;
    /*Amount of time it takes to replenish magazine*/
    public float timeToReload = 1.0f;
    /*Amount of time effects should persist for*/
    public float timeToDisplayEffects = 0.2f;
    /*Amount of time it takes to equip weapon*/
    public float timeToEquip = 0.5f;
    /*Amount of time it takes to unequip weapon*/
    public float timeToUnEquip = 0.5f;

    /*Does this weapon consume ammo*/
    public bool usesAmmo = false;
    public bool useRootTransform = false;
    public ParticleSystem gunParticles;
    public AudioSource gunAudio;
    public Light gunLight;

    /*Tracks current Weapon State*/
    public WeaponState state;

    protected int bulletsInMag;
    protected float timer;



    protected virtual void Awake()
    {
        timer = 0f;
        bulletsInMag = bulletsPerMag;
    }

    /// <summary>
    /// Increase the timer.
    /// </summary>
    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToDisplayEffects)
        {
            DisableEffects();
        }
        if(IsReloading() && timer >= timeToReload)
        {
            StopReloading();
        }
        if (timer >= timeToShoot && state == WeaponState.HipFiring)
            SetCurrentState(WeaponState.Idle);
        
    }

    public void SetCurrentState(WeaponState newState)
    {
        state = newState;
    }

    public WeaponState GetCurrentState()
    {
        return state;
    }


    public virtual bool CanShoot()
    {
        return state >= WeaponState.Idle && (HasBullets() || !usesAmmo);
    }



    public virtual bool CanActivate()
    {
        return CanShoot();
    }

    public virtual void StartReloading()
    {
        timer = 0f;
        SetCurrentState(WeaponState.Reloading);
    }

    public virtual void StopReloading()
    {
        if(IsReloading() && timer >= timeToReload)
        {
            bulletsInMag = bulletsPerMag;
        }
        SetCurrentState(WeaponState.Idle);
    }

    public bool HasBullets()
    {
        return bulletsInMag > 0;
    }

    public int GetBulletsInMag()
    {
        return bulletsInMag;
    }

    public bool IsReloading()
    {
        return state == WeaponState.Reloading;
    }

    public virtual void EnableEffects()
    {
        if (gunAudio != null)
            gunAudio.Play();

        if (gunLight != null)
            gunLight.enabled = true;

        if (gunParticles != null)
        {
            gunParticles.Play();
        }
    }

    public virtual void DisableEffects()
    {
        if (gunLight != null)
            gunLight.enabled = false;
        if (gunAudio != null)
            gunAudio.Stop();
        if (gunParticles != null)
        {
            gunParticles.Stop();
        }
    }

    public virtual float StartEquipping()
    {
        SetCurrentState(WeaponState.Equipping);
        return timeToEquip;
    }

    public virtual float StartUnEquipping()
    {
        Deactivate();
        SetCurrentState(WeaponState.UnEquipping);
        gameObject.SetActive(false);
        return timeToUnEquip;
    }

    public abstract void Activate();
    public abstract void Deactivate();
}
