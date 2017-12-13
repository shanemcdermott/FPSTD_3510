using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayDefenseState : DefendingState
{

    public PreparingState fallbackPreparingState;

    public override GameState GetNextState()
    {
       if (GameManager.instance.GetPlayer().GetComponent<PlayerController>().health.currentHealth <= 0)
            return defeatState;
        else if ((enemyManager.GetTotalSpawned() >= waveSize && enemyManager.GetLivingCount() == 0) && GameManager.instance.GetNumWavesRemaining() > 0)
            return victoryState;
        else if (defensePoint.IsDead())
            return fallbackPreparingState;
        else if (GameManager.instance.endlessMode)
        {
            waveSize++;
            return this;
        }
        else
            return levelVictoryState;
    }
}
