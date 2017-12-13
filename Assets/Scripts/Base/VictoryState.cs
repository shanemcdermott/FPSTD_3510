using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : GameState
{
    public GameState next;

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.UpdatePhaseText("Victory!");
        //Tell hud to display rewards
        GameManager.instance.menuManager.waveCountVictory.text = "" + GameManager.instance.currentWave;
        GameManager.instance.menuManager.waveCrystals.text = "" + GameManager.instance.waveCrytals;
        GameManager.instance.menuManager.levelCrystals1.text = "" + GameManager.instance.totalCrytals;
        GameManager.instance.menuManager.OpenWaveVictory();
        GameManager.instance.waveCrytals = 0;
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
