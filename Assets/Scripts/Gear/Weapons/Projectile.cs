using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    

    public int damage = 20;
    public float radius = 10f;
    public float power = 1000f;
    public float lift = 30f;

    private GameObject creator;

    public void SetCreator(GameObject inCreator)
    {
        this.creator = inCreator;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == creator) return;

        Vector3 origin = transform.position;
        Collider[] colliders = Physics.OverlapSphere(origin, radius);
        bool bDestroy = false;
        foreach(Collider other in colliders)
        {
            if (other.gameObject == creator || other.gameObject == gameObject) continue;

            Rigidbody rbody = other.GetComponent<Rigidbody>();
            if (rbody != null)
            {

                //Collider[] colliders = Physics.OverlapSphere(impactPoint, radius);
                HealthComponent health = other.gameObject.GetComponent<HealthComponent>();
                if (health)
                {
                    health.TakeDamage(new DamageContext(creator, damage, transform.position));
                }

                rbody.AddExplosionForce(power, transform.position, radius, lift);
                Debug.Log("Collided with: " + other.gameObject.name);
                bDestroy = true;
            }
        }
        if (bDestroy)
            Destroy(gameObject);
    }
}
