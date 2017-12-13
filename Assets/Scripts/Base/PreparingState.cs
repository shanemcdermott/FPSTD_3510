using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparingState : GameState
{
    public string phaseText = "Build";
    public float preparationTime = 10f;
    public GameState nextState;

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.GetPlayer().GetComponent<PlayerController>().health.ResetHealth();
        preparationTime = 10f;
        GameManager.instance.currentWave++;
        GameManager.instance.hud.wave.text = GameManager.instance.currentWave.ToString();
        GameManager.instance.UpdatePhaseText(phaseText);
        GameManager.instance.GetEnemyManager().enabled = false;
        Debug.Log("Starting Build Phase.");
        Invoke("ConsiderStateTransition", preparationTime);
    }

    public void Update()
    {
        // Display time left
        GameManager.instance.hud.time.text = preparationTime.ToString("F2");
        if (preparationTime <= 0)
        {
            GameManager.instance.hud.time.text = "";
        }
        else
        {
            preparationTime -= Time.deltaTime;
        }
    }

    public override GameState GetNextState()
    {
        return nextState;
    }

    public override bool ShouldChangeState()
    {
        return true;

    }

    public override void Exit()
    {
        base.Exit();
        CancelInvoke();
    }
}
