using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : GameState
{

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.hud.phase.text = "Victory!";
        //Tell hud to display rewards
        GameManager.instance.menuManager.waveCountVictory.text = "" + GameManager.instance.currentWave;
        GameManager.instance.menuManager.waveCrystals.text = "0";
        GameManager.instance.menuManager.levelCrystals1.text = "0";
        GameManager.instance.menuManager.OpenWaveVictory();
    }

}
