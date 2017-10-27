using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatState : GameState
{
    public override void Enter()
    {
        base.Enter();
        GameManager.instance.hud.phase.text = "Defeat!";
        //Tell hud to display rewards
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
