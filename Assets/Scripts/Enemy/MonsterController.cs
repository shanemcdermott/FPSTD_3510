using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles a monster's sub processes.
public class MonsterController : MonoBehaviour, IRespondsToDeath
{

    public Team team;
    public HealthComponent health;
    public EnemyMovement movement;
    public EnemyAttack attack;

    //Whatever the current focused object is
    GameObject target;

    void Awake()
    {
        team = GetComponent<Team>();
        health = GetComponent<EnemyHealth>();

        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();

        health.RegisterDeathResponder(this);
        health.RegisterDeathResponder(movement);
        health.RegisterDeathResponder(attack);
    }
    private void Start()
    {
        FindNextTarget();
    }

    public void FindNextTarget()
    {
        AssignTarget(GameObject.FindGameObjectWithTag("Player"));
        /*
        if (Random.Range(0, 100) > 75)
            AssignTarget(GameObject.FindGameObjectWithTag("Player"));
        else
            AssignTarget(GameObject.FindGameObjectWithTag("Tower"));
        */
    }

    void AssignTarget(GameObject nextTarget)
    {
        movement.AssignTarget(nextTarget);
        attack.AssignTarget(nextTarget);
    }

    // Update is called once per frame
    void Update ()
    {
		if(!health.IsDead())
        {
            //Choose Behavior
            //Handle decision making       
        }
	}
    public void OnDeath(DamageContext context)
    {
        
        GetComponent<Rigidbody>().isKinematic = true;
       // isSinking = true;
        //ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }
}
