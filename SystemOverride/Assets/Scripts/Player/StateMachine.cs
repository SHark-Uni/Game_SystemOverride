using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
	EntityState<T> _currentState;

	public void BeginMachine(EntityState<T> state)
	{
		_currentState = state;
		_currentState.Enter();
	}

	public void ChangeState(EntityState<T> state)
	{
		_currentState.Exit();
		_currentState = state;
		_currentState.Enter();
	}
}
