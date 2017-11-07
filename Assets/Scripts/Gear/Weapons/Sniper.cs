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
    public Camera fpsCam;

	// Use this for initialization
    
	void Start () {
        isScoped = false;
       /* damage = 1000;
        range = 1000;
        timeToReload = 2;
        magazineCapacity = 5;
        fireRate = .75f;
        bIsReloading = false;
        bIsBusy = false;
        bulletsInMag = magazineCapacity;
        timer = 0f;
        */
    }

    public void ZoomedShoot()
    {
        shootRay.origin = fpsCam.transform.position;
        shootRay.direction = fpsCam.transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            Debug.Log("Object Hit: " + shootHit.transform.name);
            HealthComponent enemyHealth = shootHit.collider.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(new DamageContext(gameObject, damagePerShot, shootHit.point));
            }
            //traceLine.SetPosition(1, shootHit.point);
        }
        else
        {
            //traceLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    public override void StartReloading()
    {
        base.StartReloading();
        OnUnscoped();
    }



    // Update is called once per frame
    protected override void Update()
    {
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
            

            //animator.SetBool("IsScoped", isScoped);
        
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
