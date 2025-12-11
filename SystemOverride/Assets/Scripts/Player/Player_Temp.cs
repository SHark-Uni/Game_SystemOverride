using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Temp : MonoBehaviour
{
	Vector2 _playerInput;
	PlayerInput _Input;

	private void Awake()
	{
		_Input = new PlayerInput();	
	}

	void Start()
    {
        
    }

	private void OnEnable()
	{
		_Input.Enable();

		_Input.Player.Move.performed += ctx => _playerInput = ctx.ReadValue<Vector2>();
		_Input.Player.Move.canceled += ctx => _playerInput = Vector2.zero;
	}


	void Update()
    {
		if (_playerInput.x == 1)
		{
			Debug.Log("오른쪽");
		}

		if (_playerInput.x == -1)
		{
			Debug.Log("왼쪽");
		}
	}
}
