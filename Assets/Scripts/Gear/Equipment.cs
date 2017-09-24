using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Abstract parent class for game equipment.
/// </summary>
public abstract class Equipment : MonoBehaviour {

    protected bool bIsBusy;
 
    public virtual bool IsBusy()
    {
        return bIsBusy;
    }

    public void SetIsBusy(bool bNewBusy)
    {
        bIsBusy = bNewBusy;
    }

    public abstract bool CanActivate();
    public abstract void Activate();
    public abstract void Deactivate();

    public abstract void EnableEffects();
    public abstract void DisableEffects();
}
