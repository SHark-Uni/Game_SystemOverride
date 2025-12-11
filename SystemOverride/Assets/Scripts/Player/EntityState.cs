using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState<T>
{
	protected T _owner;
	protected StateMachine<T> _stateMachine;
	protected string _name;
	protected Rigidbody2D _rb;
	protected Animator _am;

	protected bool _trigger;
	protected float _duration;

	public EntityState(T owner, StateMachine<T> stateMachine, string name, Rigidbody2D rb, Animator am)
	{
		_owner = owner;
		_stateMachine = stateMachine;
		_name = name;
		_rb = rb;
		_am = am;

		_trigger = false;
		_duration = 0;
	}

	public virtual void Enter()
	{
		_trigger = false;
		_am.SetBool(_name, true);
	}

	public virtual void EntityUpdate()
	{ 
		
	}
	public virtual void Exit()
	{
		_am.SetBool(_name, false);
	}

	public void SetTrigger()
	{
		_trigger = true;
	}
}
