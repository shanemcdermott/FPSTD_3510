using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVictoryState : GameState {

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.hud.phase.text = "Level Victory!";
        //Tell hud to display rewards
        GameManager.instance.menuManager.levelCrystals2.text = "0";
        GameManager.instance.menuManager.OpenLevelVictory();
    }
}
