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
    public GameObject player;

	// Use this for initialization
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {
        healthSlider.value = player.GetComponent<PlayerHealth>().currentHealth;

        if (player.GetComponent<PlayerController>().isPlacing)
        {
            phase.text = "Build";
        }
        else
        {
            phase.text = "Defend";
        }
	}


}
