using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerSuperState : EntityState<Player_Temp>
{
	protected PlayerInput.PlayerActions _inputAction;
    // 대시, 백대시, 앉기 상태용 New Input System 생성
    protected PlayerMove.PlayerActions _playerMove;

    public PlayerSuperState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am) 
		: base(owner, stateMachine, name, rb, am)
	{
		_inputAction = _owner.Input.Player;
        // 대시, 백대시, 앉기 상태용 New Input System 생성
        _playerMove = _owner.playerMove.Player;

    }

	public override void Enter()
	{
		base.Enter();
	}

	public override void EntityUpdate()
	{
		base.EntityUpdate();
		_am.SetFloat("yVelocity", _rb.velocity.y);
	}

	public override void Exit()
	{
		base.Exit();
	}


}
