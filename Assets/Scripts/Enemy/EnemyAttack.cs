using UnityEngine;
using System.Collections;
using System;

public class EnemyAttack : MonoBehaviour, IRespondsToDeath
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    public Team team;


    Animator anim;
    GameObject target;
    HealthComponent targetHealth;
    bool enemyInRange;
    float timer;
    protected MonsterController controller;

    void Awake ()
    {
        team = GetComponent<Team>();
        anim = GetComponent <Animator> ();
        controller = GetComponent<MonsterController>();
    }

    public void AssignTarget(GameObject targetObject)
    {
        
        target = targetObject;
        if(target != null)
            targetHealth = target.GetComponent<HealthComponent>();
    }

    //Attack any overlapping enemies
    void OnTriggerEnter (Collider other)
    {
        Team otherTeam = other.GetComponent<Team>();
        if(otherTeam != null && !otherTeam.IsFriendly(team))
        {
            GetComponent<Animator>().SetBool("isAttacking", true);
            enemyInRange = true;
            AssignTarget(other.gameObject);
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == target)
        {
            GetComponent<Animator>().SetBool("isAttacking", false);
            enemyInRange = false;
            controller.FindNextTarget();
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && enemyInRange)
        {
            Attack ();
        }

        
        if(targetHealth != null && targetHealth.IsDead())
        {
            controller.FindNextTarget();
            /*
            if (target.tag == "Player")
            {
                anim.SetTrigger("PlayerDead");
            }
            else
            {
                controller.FindNextTarget();
            }
            */
        }
        
    }


    void Attack ()
    {
        timer = 0f;

        if(!targetHealth.IsDead())
        {
            targetHealth.TakeDamage (new DamageContext(gameObject, attackDamage));
        }
    }

    public void OnDeath(DamageContext context)
    {
        enabled = false;
    }
}
