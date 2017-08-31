using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform target;
    HealthComponent targetHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;


    void Awake ()
    {
        AssignTarget(GameObject.FindGameObjectWithTag("Tower"));   
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }

    void AssignTarget(GameObject targetObject)
    {
        target = targetObject.transform;
        targetHealth = target.GetComponent<HealthComponent>();
    }

    //Using Update instead of fixed update since this is a navmesh agent
    void Update ()
    {
        if(enemyHealth.currentHealth > 0 && !targetHealth.IsDead())
        {
            nav.SetDestination (target.position);
        }
        else
        {
            nav.enabled = false;
        }
    }
}
