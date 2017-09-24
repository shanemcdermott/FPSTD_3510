using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    Team team;
    HealthComponent health;
    EnemyMovement movement;
    EnemyAttack attack;

    //Whatever the current focused object is
    GameObject target;

    void Awake()
    {
        team = GetComponent<Team>();
        health = GetComponent<HealthComponent>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
    }

    private void Start()
    {
        FindNextTarget();
    }

    public void FindNextTarget()
    {
        if (Random.Range(0, 100) > 75)
            AssignTarget(GameObject.FindGameObjectWithTag("Player"));
        else
            AssignTarget(GameObject.FindGameObjectWithTag("Tower"));
    }

    void AssignTarget(GameObject nextTarget)
    {
        movement.AssignTarget(nextTarget);
        attack.AssignTarget(nextTarget);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
