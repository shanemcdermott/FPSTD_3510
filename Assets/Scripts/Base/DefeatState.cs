using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatState : GameState
{
    public GameState next;

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.UpdatePhaseText("Defeat!");
        //Tell hud to display rewards
        GameManager.instance.menuManager.waveCountDefeat.text = "" + GameManager.instance.currentWave;
        GameManager.instance.menuManager.OpenWaveDefeat();
        GameManager.instance.currentWave--;
        GameManager.instance.GetEnemyManager().clearEnemies();
        GameManager.instance.GetPlayer().GetComponent<PlayerController>().health.ResetHealth();
        Invoke("ConsiderStateTransition", 1f);
    }

    public override GameState GetNextState()
    {
        return next;
    }

    public override bool ShouldChangeState()
    {
        return true;
    }
}
