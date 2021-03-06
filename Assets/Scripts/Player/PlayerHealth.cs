﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : HealthComponent
{
    
    //public Slider healthSlider;
   // public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;
    AudioSource playerAudio;
    bool isDead;
    bool damaged;


    void Awake ()
    {
        base.ResetHealth();
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();

        //healthSlider.value = currentHealth;
    }


    void Update ()
    {
        if(damaged)
        {
           // damageImage.color = flashColour;
        }
        else
        {
          //  damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public override void TakeDamage(DamageContext context)
    {
        if (!CanTakeDamage(context))
            return;
        base.TakeDamage(context);

        damaged = true;
        //healthSlider.value = currentHealth;
        playerAudio.Play();
    }

    public override void OnDeath(DamageContext context)
    {
        base.OnDeath(context);
        anim.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();

    }

    public void RestartLevel ()
    {
        //SceneManager.LoadScene (0);
    }
}
