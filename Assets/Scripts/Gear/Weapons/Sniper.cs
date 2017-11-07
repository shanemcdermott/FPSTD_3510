using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : TraceWeapon
{
    public Animator animator;

    public GameObject scopeOverlay;
    public GameObject crosshair;
    public GameObject weaponCamera;

    public float scopedFOV = 15f;
    private float normalFOV;

    private bool isScoped = false;
    public Camera mainCamera;

	// Use this for initialization
    
	void Start() {
        isScoped = false;
        damagePerShot = 1000;
        range = 1000;
        timeToReload = 2;
        bulletsPerMag = 5;
        timeToShoot = 0.9f;
        bulletsInMag = bulletsPerMag;
    }
    public override void StartReloading()
    {
        base.StartReloading();
        OnUnscoped();
    }



    // Update is called once per frame
    protected override void Update()
    {
        animator.SetBool("IsScoped", isScoped);
        base.Update();
        if (Input.GetButtonDown("Fire2"))
        {
            if (!isScoped)
            {
                Debug.Log("scoping");
                StartCoroutine(OnScoped());
            }
            else
            {
                Debug.Log("unscoping");
                OnUnscoped();
            }
        }
    }

    void OnUnscoped()
    {
        scopeOverlay.SetActive(false);
        weaponCamera.SetActive(true);
        crosshair.SetActive(true);

        mainCamera.fieldOfView = normalFOV;
        isScoped = false;
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);

        scopeOverlay.SetActive(true);
        weaponCamera.SetActive(false);
        crosshair.SetActive(false);

        normalFOV = mainCamera.fieldOfView;
        mainCamera.fieldOfView = scopedFOV;
        isScoped = true;
    }
}
