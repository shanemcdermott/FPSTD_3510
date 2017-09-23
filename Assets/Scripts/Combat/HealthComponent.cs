using System.Collections;
using System.Collections.Generic;
using UnityEngine;





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
