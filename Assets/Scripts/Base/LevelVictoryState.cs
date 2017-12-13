using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVictoryState : GameState {

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.hud.phase.text = "Level Victory!";
        //Tell hud to display rewards
        GameManager.instance.menuManager.levelCrystals2.text = "" + GameManager.instance.totalCrytals;
        GameManager.instance.menuManager.OpenLevelVictory();
    }

    public override GameState GetNextState()
    {
        throw new System.NotImplementedException();
    }

    public override bool ShouldChangeState()
    {
        throw new System.NotImplementedException();
    }
}
