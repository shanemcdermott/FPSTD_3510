using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : TraceWeapon {

    // Use this for initialization
    void Start () {
        //damagePerShot = 100;
        //range = 300;
        //timeToReload = 1;
        //magazineCapacity = 30;
        //fireRate = .15f;
        //bIsReloading = false;
        //bIsBusy = false;
        //bulletsInMag = magazineCapacity;
        //timer = 0f;
    }

    // Update is called once per frame
    /*
    public override void Activate()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Object Hit: " + hit.transform.name);
            MonsterController enemy = hit.transform.GetComponent<MonsterController>();
            enemy.TakeDamage(damage);
        }
        //TraceWeapon trace = this.GetComponent<TraceWeapon>();
        //trace.Activate();
    }
    */

}
