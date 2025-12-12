using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayrAirState
{
	public JumpState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am) 
		: base(owner, stateMachine, name, rb, am)
	{
	}

	public override void Enter()
	{
		base.Enter();
		_owner.SetVelocity(0 , _owner.jumpForce);
	}
	public override void EntityUpdate()
	{
		base.EntityUpdate();


		if (_rb.velocity.y < 0)
		{
			_stateMachine.ChangeState(_owner.fallState);
		}
	}

	public override void Exit()
	{
		base.Exit();
	}
}
