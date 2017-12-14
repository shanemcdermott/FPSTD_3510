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
    public LevelVictoryState levelVictoryState;
    
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
        bool result = (enemyManager.GetTotalSpawned() >= waveSize 
            && enemyManager.GetLivingCount() == 0) || GameManager.instance.GetPlayer().GetComponent<PlayerController>().health.currentHealth <= 0;
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
        else if (GameManager.instance.GetPlayer().GetComponent<PlayerController>().health.currentHealth <= 0)
            return defeatState;
        else if ((enemyManager.GetTotalSpawned() >= waveSize && enemyManager.GetLivingCount() == 0) && GameManager.instance.GetNumWavesRemaining() > 0)
            return victoryState;
        else
            return levelVictoryState;
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
