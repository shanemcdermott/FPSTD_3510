using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour, IState
{
    public virtual void Enter()
    {
        enabled = true;
    }



    /// <summary>
    /// Considers whether to change states.
    /// Also handles state transition.
    /// </summary>
    protected virtual void ConsiderStateTransition()
    {
        if (ShouldChangeState())
            GameManager.instance.SetState(GetNextState());
    }

    public abstract bool ShouldChangeState();

    //Handled by children.
    public abstract GameState GetNextState();

    public virtual void Exit()
    {
        enabled = false;
        //
    }
}
