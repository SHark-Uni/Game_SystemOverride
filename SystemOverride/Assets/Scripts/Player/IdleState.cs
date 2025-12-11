using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerSuperState
{
	public IdleState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am) 
		: base(owner, stateMachine, name, rb, am)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void EntityUpdate()
	{
		base.EntityUpdate();

		//키입력을 한다면, Walk 상태로 전파
		if (_owner.playerInput.x != 0)
		{
			_stateMachine.ChangeState(_owner.walkState);
		}
	}

	public override void Exit()
	{
		base.Exit();
	}
}
