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

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }


	// Update is called once per frame
	void Update () {
        healthSlider.value = player.GetComponent<PlayerHealth>().currentHealth;
	}


}
