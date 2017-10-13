using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    private PlayerHealth playerHealth;
    public Slider healthSlider;
    public Text wave;
    public Text crystals;
    public Text upgrades;
    public Text phase;

	// Use this for initialization
	void Start () {
        playerHealth = GetComponent<PlayerHealth>();
        
	}
	
	// Update is called once per frame
	void Update () {
        healthSlider.value = playerHealth.currentHealth;
	}


}
