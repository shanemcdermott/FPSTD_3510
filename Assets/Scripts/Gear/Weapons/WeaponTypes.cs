using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    rifle, sniper, shotgun
}

public enum WeaponState
{
    /*Weapon is not currently equipped*/
    Unequipped,
    /*Weapon is being equipped*/
    Equipping,
    /*Weapon is being unequipped*/
    UnEquipping,
    /*Weapon is being fired from the hip*/
    HipFiring,
    /*Weapon is being fired while scoped*/
    ScopedFiring,
    /*Weapon is being reloaded*/
    Reloading,
    /*Weapon is equipped but not busy*/
    Idle,
    /*Weapon is equipped and scoped*/
    Scoped,
}