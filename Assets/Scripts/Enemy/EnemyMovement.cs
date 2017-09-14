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
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }

    public void AssignTarget(GameObject targetObject)
    {
        target = targetObject.transform;
        targetHealth = target.GetComponent<HealthComponent>();
    }

    //Using Update instead of fixed update since this is a navmesh agent
    void Update ()
    {
        if(enemyHealth.currentHealth > 0 && targetHealth != null && !targetHealth.IsDead())
        {
            nav.SetDestination (target.position);
        }
    }
}
