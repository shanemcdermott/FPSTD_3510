using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour, IState
{
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        Destroy(this);
        //
    }
}
