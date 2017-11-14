using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, Equipment
{
    public Camera mainCamera;
    public Transform aimTransform;
    public Animator animator;
    /*Maximum Number of bullets in a magazine*/
    public int bulletsPerMag = 100;

    /*Amount of time it takes to fire one shot.*/
    public float timeToShoot = 0.15f;
    /*Amount of time it takes to replenish magazine*/
    public float timeToReload = 1.0f;
    /*Amount of time effects should persist for*/
    public float timeToDisplayEffects = 0.1f;
    /*Amount of time it takes to equip weapon*/
    public float timeToEquip = 0.5f;
    /*Amount of time it takes to unequip weapon*/
    public float timeToUnEquip = 0.5f;

    /*Does this weapon consume ammo*/
    public bool usesAmmo = true;
    public ParticleSystem gunParticles;
    public AudioSource gunAudio;
    public AudioSource reloadAudio;
    public Light gunLight;

    /*Tracks current Weapon State*/
    public WeaponState state;

    public int bulletsInMag;
    protected float shootTimer;
    protected float reloadTimer;

    private Transform recoilMod;
    private float maxRecoil_x = -20;
    private float recoilSpeed = 10;
    public float recoil = 0.0f;


    protected virtual void Awake()
    {
        shootTimer = 0f;
        reloadTimer = 0f;
        bulletsInMag = bulletsPerMag;
        usesAmmo = true;
        recoilMod = new GameObject().transform;
        aimTransform = mainCamera.transform;
    }

    /// <summary>
    /// Increase the timer.
    /// </summary>
    protected virtual void Update()
    {
        shootTimer += Time.deltaTime;
        reloadTimer += Time.deltaTime;
        if (shootTimer >= timeToDisplayEffects)
        {
            DisableEffects();
        }
        if(IsReloading() && reloadTimer >= timeToReload)
        {
            StopReloading();
        }
        if (shootTimer >= timeToShoot && state == WeaponState.HipFiring)
            SetCurrentState(WeaponState.Idle);
        
    }

    public void Recoil()
    {
        if (recoil > 0)
        {
            var maxRecoil = Quaternion.Euler(maxRecoil_x, 0, 0);
            // Dampen towards the target rotation
            recoilMod.rotation = Quaternion.Slerp(recoilMod.rotation, maxRecoil, Time.deltaTime * recoilSpeed);
            mainCamera.transform.Rotate(recoilMod.rotation.eulerAngles);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0;
            var minRecoil = Quaternion.Euler(0, 0, 0);
            recoilMod.rotation = Quaternion.Slerp(recoilMod.rotation, minRecoil, Time.deltaTime * recoilSpeed / 2);
            mainCamera.transform.Rotate(recoilMod.rotation.eulerAngles);
        }
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
        return state >= WeaponState.Idle && (HasBullets() || !usesAmmo) && shootTimer >= timeToShoot;
    }



    public virtual bool CanActivate()
    {
        return CanShoot();
    }

    public virtual void StartReloading()
    {
        reloadTimer = 0f;
        SetCurrentState(WeaponState.Reloading);
        animator.SetBool("IsReloading", true);
        reloadAudio.Play();
    }

    public virtual void StopReloading()
    {
        bulletsInMag = bulletsPerMag;
        SetCurrentState(WeaponState.Idle);
        animator.SetBool("IsReloading", false);
        reloadAudio.Stop();
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
