using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayrAirState : PlayerSuperState
{
	float AirMoveSpeedX;

	public PlayrAirState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am) 
		: base(owner, stateMachine, name, rb, am)
	{
		AirMoveSpeedX = _owner.moveSpeed.x * _owner.airMoveMultiplier;
	}


	public override void Enter()
	{
		base.Enter();
	}

	public override void EntityUpdate()
	{
		base.EntityUpdate();

		if (_owner.playerInput.x != 0)
		{
			_owner.SetVelocity(_owner.playerInput.x * AirMoveSpeedX, _rb.velocity.y);
		}
	}
	public override void Exit()
	{
		base.Exit();
	}
}
