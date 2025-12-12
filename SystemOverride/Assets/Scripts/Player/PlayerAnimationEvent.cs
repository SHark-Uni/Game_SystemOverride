using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
	Player_Temp _player;

	private void Start()
	{
		_player = GetComponentInParent<Player_Temp>();
	}

	public void OnAttackEnd()
	{
		_player.SetAnimTrigger();
	}
}
