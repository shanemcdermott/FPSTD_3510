using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendingState : GameState
{
    private EnemyManager enemyManager;

    public override void Enter()
    {
        base.Enter();
        enemyManager = GameManager.instance.GetEnemyManager();
        enemyManager.enabled = true;
        GameManager.instance.HUD.phase.text = "Defend!";
        Debug.Log("Starting Defend Phase.");
    }

    public override void Exit()
    {
        enemyManager.enabled = false;
        
        base.Exit();
        
    }
}
