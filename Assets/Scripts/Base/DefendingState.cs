using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendingState : GameState
{
    //Number of enemies to spawn in each wave.
    public int waveSize;
    public PreparingState buildState;
    public DefeatState defeatState;
    public VictoryState victoryState;

    private EnemyManager enemyManager;

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.hud.time.text = "";
        enemyManager = GameManager.instance.GetEnemyManager();
        enemyManager.waveSize = waveSize;
        enemyManager.enabled = true;
        GameManager.instance.UpdatePhaseText("Defend!");
        Debug.Log("Starting Defend Phase.");
        InvokeRepeating("ConsiderStateTransition", 1f, 1f);
    }

    public override bool ShouldChangeState()
    {
        bool result = enemyManager.GetTotalSpawned() >= waveSize 
            && enemyManager.GetLivingCount() == 0;
        //Debug.Log("Should change state: " + result.ToString());
        return result;
    }

    public override GameState GetNextState()
    {
        if (GameManager.instance.endlessMode)
        {
            waveSize++;
            return this;
        }
        else if (GameManager.instance.GetNumWavesRemaining() > 1)
            return victoryState;
        else
            return defeatState;
    }

    //TODO
    public virtual bool AreObjectivesComplete()
    {
        return true;
    }


    public override void Exit()
    {
        enemyManager.enabled = false;
        CancelInvoke();
        base.Exit();
        
    }

}
