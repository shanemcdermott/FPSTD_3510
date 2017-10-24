using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparingState : GameState
{
    public float preparationTime;
    public GameState nextState;

    private float _timer;
	// Use this for initialization
	void Start ()
    {
        _timer = 0f;
	}

    public override void Enter()
    {
        base.Enter();
        _timer = 0f;
        GameManager.instance.HUD.phase.text = "Build";
        GameManager.instance.GetEnemyManager().enabled = false;
        Debug.Log("Starting Build Phase.");
    }

    // Update is called once per frame
    void Update ()
    {
        _timer += Time.deltaTime;
        if (_timer >= preparationTime)
            GameManager.instance.SetState(nextState);    	
	}
}
