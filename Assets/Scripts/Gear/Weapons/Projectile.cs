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
    private ParticleSystem explosion;
    public float multiplier = 1;

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
            if (other.gameObject.transform.tag == "Tile") bDestroy = true;
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
                Debug.Log(gameObject + " collided with: " + other.gameObject.name);
                bDestroy = true;
            }
        }
        if (bDestroy)
        {
            StartCoroutine("PlayParticles");
        }
    }


    IEnumerator PlayParticles()
    {
        var systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            system.Clear();
            system.Play();

        }
        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
        foreach (ParticleSystem system in systems)
        {
            system.Stop();
        }
    }
}
