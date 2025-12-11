using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AttackState : PlayerSuperState
{
	public AttackState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am) 
		: base(owner, stateMachine, name, rb, am)
	{
	}

	public override void Enter()
	{
		base.Enter();
		SpawnBullet();
	}

	public override void EntityUpdate()
	{
		base.EntityUpdate();

		//공격키 누르면 총알 나감.

		if (_trigger == true)
		{
			_stateMachine.ChangeState(_owner.idleState);
		}
	}
	public override void Exit()
	{
		base.Exit();
	}

	private void SpawnBullet()
	{
		GameObject bullet;
		_owner.Shoot(out bullet);

		bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
	}

}
