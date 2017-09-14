using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour {

    protected bool bIsBusy;


	// Update is called once per frame
	void Update ()
    {
		
	}

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
