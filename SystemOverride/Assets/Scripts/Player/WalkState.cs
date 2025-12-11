using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerSuperState
{
	public WalkState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am) 
		: base(owner, stateMachine, name, rb, am)
	{
	}

	// Start is called before the first frame update
	public override void Enter()
	{
		base.Enter();
	}
	public override void EntityUpdate()
	{
		base.EntityUpdate();

		if (_owner.playerInput.x == 0)
		{
			_stateMachine.ChangeState(_owner.idleState);
		}
		_owner.SetVelocity(_owner.playerInput.x * 5, _rb.velocity.y);
	}

	public override void Exit()
	{
		base.Exit();
	}
}
