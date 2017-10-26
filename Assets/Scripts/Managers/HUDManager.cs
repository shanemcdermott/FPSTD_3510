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
    private GameObject player;

    void Start()
    {
        GameManager.instance.hud = this;
        player = GameManager.instance.GetPlayer();
    }


	// Update is called once per frame
	void Update () {
        healthSlider.value = player.GetComponent<PlayerHealth>().currentHealth;
	}


}
