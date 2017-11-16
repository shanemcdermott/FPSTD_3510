using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatState : GameState
{
    public override void Enter()
    {
        base.Enter();
        GameManager.instance.UpdatePhaseText("Defeat!");
        //Tell hud to display rewards
        GameManager.instance.menuManager.waveCountDefeat.text = "" + GameManager.instance.currentWave;
        GameManager.instance.menuManager.OpenWaveDefeat();
    }

    public override GameState GetNextState()
    {
        throw new NotImplementedException();
    }

    public override bool ShouldChangeState()
    {
        return false;
    }
}
