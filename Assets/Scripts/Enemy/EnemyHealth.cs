using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : HealthComponent
{

    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
    public Text health;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
    }

    private void Start()
    {
        base.ResetHealth();
    }

    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
        health.text = this.currentHealth + "/" + this.startingHealth;
    }


    public override void TakeDamage (DamageContext context)
    {
        if (!CanTakeDamage(context))
            return;

        base.TakeDamage(context);
        if (enemyAudio != null)
        {
            enemyAudio.Play();
        }
        if (hitParticles != null)
        {
            hitParticles.transform.position = context.hitLocation;
            hitParticles.Play();
        }
        GetComponent<Animator>().SetTrigger("hit");
    }


    public override void OnDeath (DamageContext context)
    {
        base.OnDeath(context);

        
        GetComponent<Animator>().SetBool("isDead", true);
        GameManager.instance.crystals += 30;
        GameManager.instance.waveCrytals += 30;
        GameManager.instance.totalCrytals += 30;
        if (enemyAudio != null)
        {
            if (enemyAudio.isPlaying)
                enemyAudio.Stop();

            enemyAudio.PlayOneShot(deathClip);
        }
    }
}
