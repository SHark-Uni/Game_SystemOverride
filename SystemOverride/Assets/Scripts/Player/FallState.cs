using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayrAirState
{
	public FallState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am) 
		: base(owner, stateMachine, name, rb, am)
	{
	}

	public override void EntityUpdate()
	{
		base.EntityUpdate();

		if (_owner.onGround)
		{
			_stateMachine.ChangeState(_owner.idleState);
			_owner.AvailableDoubleJump();
		}
	}

}
