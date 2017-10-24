using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{

    void Enter();
    void Exit();
}

public interface IStateController<T> where T : IState
{
    void SetState(T state);
    T GetState();

}