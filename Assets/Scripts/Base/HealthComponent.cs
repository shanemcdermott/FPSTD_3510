using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageContext
{
    public GameObject source;
    public int amount;
    public int type;
    public Vector3 hitLocation;

    public DamageContext(GameObject source, int amount)
    {
        this.source = source;
        this.amount = amount;
        this.type = 0;
        this.hitLocation = new Vector3();
    }

    public DamageContext(GameObject source, int amount, Vector3 hitLocation)
    {
        this.source = source;
        this.amount = amount;
        this.type = 0;
        this.hitLocation = hitLocation;
    }

    public DamageContext(GameObject source, int amount, Vector3 hitLocation, int type)
    {
        this.source = source;
        this.amount = amount;
        this.type = type;
        this.hitLocation = hitLocation;
    }
}

public class HealthComponent : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    bool bIsDead;

    void Awake()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;
        bIsDead = false;
    }

    public virtual bool CanTakeDamage(DamageContext context)
    {
        return !bIsDead;
    }

    public bool IsDead()
    {
        return bIsDead;
    }

    public virtual void TakeDamage(DamageContext context)
    {
        if(CanTakeDamage(context))
        {
            currentHealth -= context.amount;
            if (currentHealth <= 0 && !bIsDead)
            {
                Death();
            }
        }
    }

    protected virtual void Death()
    {
        bIsDead = true;
    }
}
