using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Abstract parent class for game equipment.
/// </summary>
public interface Equipment
{
    //Start playing equip animation. Return the time it will take to equip.
    float StartEquipping();
    //Start playing unequip animation. Return the time it will take to unequip.
    float StartUnEquipping();

    bool CanActivate();
    void Activate();
    void Deactivate();

    void EnableEffects();
    void DisableEffects();
}
