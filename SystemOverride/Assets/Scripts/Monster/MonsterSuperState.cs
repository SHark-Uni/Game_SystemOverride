using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterSuperState
{
    protected Monster _monster;
    protected MonsterStateMachine _stateMachine;
    protected string _animationName;
    public MonsterSuperState(Monster monster, MonsterStateMachine stateMachine, string animationName)
    {
        this._monster = monster;
        this._stateMachine = stateMachine;
        this._animationName = animationName;
    }

    public virtual void OnEnter()
    {
        if (!string.IsNullOrEmpty(_animationName))
        { 
            _monster._animator.SetBool(_animationName, true);
        }

    }

    public abstract void OnUpdate();

    public virtual void OnExit()
    {
        if (!string.IsNullOrEmpty(_animationName))
        {
            _monster._animator.SetBool(_animationName, false);
        }
    }
}
