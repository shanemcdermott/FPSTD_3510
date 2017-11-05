using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparingState : GameState
{
    public float preparationTime = 10f;
    public GameState nextState;

    public override void Enter()
    {
        base.Enter();
        Invoke("ConsiderStateTransition", preparationTime);
        GameManager.instance.hud.phase.text = "Build";
        GameManager.instance.GetEnemyManager().enabled = false;
        Debug.Log("Starting Build Phase.");
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
