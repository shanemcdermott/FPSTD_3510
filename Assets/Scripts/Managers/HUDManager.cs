using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    //private PlayerHealth playerHealth;
    public Slider healthSlider;
    public Text wave;
    public Text crystals;
    public Text upgrades;
    public Text phase;
    public Text time;
    public Text ammo;
    public Text enemies;
    private GameObject player;

    void Start()
    {
        GameManager.instance.hud = this;
        player = GameManager.instance.GetPlayer();
    }


	// Update is called once per frame
	void Update () {
        
        healthSlider.value = player.GetComponent<PlayerHealth>().currentHealth;
        upgrades.text = "" + GameManager.instance.upgrades;
        crystals.text = "" + GameManager.instance.crystals;
        ammo.text = player.GetComponent<PlayerController>().currentWeapon.bulletsInMag + "/" + player.GetComponent<PlayerController>().currentWeapon.bulletsPerMag;
        enemies.text = "" + GameManager.instance.GetEnemyManager().waveSize;
	}


}
