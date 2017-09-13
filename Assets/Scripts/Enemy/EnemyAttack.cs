using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    public Team team;


    Animator anim;
    GameObject target;
    HealthComponent targetHealth;
    EnemyHealth enemyHealth;
    bool enemyInRange;
    float timer;


    void Awake ()
    {
        team = GetComponent<Team>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }

    public void AssignTarget(GameObject targetObject)
    {
        target = targetObject;
        targetHealth = target.GetComponent<HealthComponent>();
    }

    //Attack any overlapping enemies
    void OnTriggerEnter (Collider other)
    {
        Team otherTeam = other.GetComponent<Team>();
        if(otherTeam != null && !otherTeam.IsFriendly(team))
        {
            enemyInRange = true;
            AssignTarget(other.gameObject);
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == target)
        {
            enemyInRange = false;
            GetComponent<MonsterController>().FindNextTarget();
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && enemyInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        
        if(targetHealth.IsDead())
        {
            if (target.tag == "Player")
            {
                anim.SetTrigger("PlayerDead");
            }
            else
            {
                GetComponent<MonsterController>().FindNextTarget();
            }
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
