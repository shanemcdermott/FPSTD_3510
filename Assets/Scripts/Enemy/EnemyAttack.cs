using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    Animator anim;
    GameObject target;
    HealthComponent targetHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;


    void Awake ()
    {
        AssignTarget(GameObject.FindGameObjectWithTag("Tower"));

        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }

    void AssignTarget(GameObject targetObject)
    {
        target = targetObject;
        targetHealth = target.GetComponent<HealthComponent>();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == target)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == target)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        
        if(targetHealth.IsDead())
        {
            anim.SetTrigger ("PlayerDead");
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
}
