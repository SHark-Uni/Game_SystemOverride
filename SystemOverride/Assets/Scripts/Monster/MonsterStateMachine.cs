using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    public MonsterSuperState CurrentState { get; private set; }

    public void Initialize(MonsterSuperState startState)
    {
        CurrentState = startState;
        //CurrentState.OnEnter();
    }

    public void ChangeState(MonsterSuperState changeState)
    {
        CurrentState.OnExit();

        CurrentState = changeState;

        CurrentState.OnEnter();
    }

   

}