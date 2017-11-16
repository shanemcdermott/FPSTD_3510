using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : GameState
{

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.UpdatePhaseText("Victory!");
        //Tell hud to display rewards
        GameManager.instance.menuManager.waveCountVictory.text = "" + GameManager.instance.currentWave;
        GameManager.instance.menuManager.waveCrystals.text = "0";
        GameManager.instance.menuManager.levelCrystals1.text = "0";
        GameManager.instance.menuManager.OpenWaveVictory();
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
