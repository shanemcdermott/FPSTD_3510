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

    public virtual void Exit()
    {
        enabled = false;
        //
    }
}
