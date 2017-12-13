using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class HealthComponent : MonoBehaviour, IRespondsToDeath
{

    public int startingHealth = 100;
    public int currentHealth;
    bool bIsDead;
    public int teamId;
    bool bEnableFriendlyFire;
    //Array of components that need to do something when the object dies
    protected List<IRespondsToDeath> deathResponders = new List<IRespondsToDeath>();

    void Awake()
    {
        ResetHealth();
    }

    //Add deathResponder to collection of responders to notify upon death
    public void RegisterDeathResponder(IRespondsToDeath deathResponder)
    {
        deathResponders.Add(deathResponder);
    }

    public void RegisterTeam(int teamId, bool enableFriendlyFire)
    {
        this.teamId = teamId;
        this.bEnableFriendlyFire = enableFriendlyFire;
    }


    public void ResetHealth()
    {
        currentHealth = startingHealth;
        bIsDead = false;
    }

    public virtual bool CanTakeDamage(DamageContext context)
    {
        if(!bIsDead)
        {
            if (bEnableFriendlyFire) return true;

            Team sourceTeam = context.source.GetComponent<Team>();
            return (sourceTeam == null || sourceTeam.id != teamId);
        }
        return false;
    }

    public bool IsDead()
    {
        return bIsDead;
    }

    public void kill()
    {
        bIsDead = true;
    }

    public virtual void TakeDamage(DamageContext context)
    {
        if(CanTakeDamage(context))
        {
            currentHealth -= context.amount;
            Debug.Log(gameObject + " Took " + context.amount + " damage from " + context.source);
            if (currentHealth <= 0 && !bIsDead)
            {
                OnDeath(context);
            }
        }
    }

    //Enable bIsDead flag and notify all responders of death
    public virtual void OnDeath(DamageContext context)
    {
        bIsDead = true;
        foreach(IRespondsToDeath responder in deathResponders)
        {
            responder.OnDeath(context);
        }
    }
}
