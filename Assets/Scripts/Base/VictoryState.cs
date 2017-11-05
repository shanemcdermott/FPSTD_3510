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
    }

}
