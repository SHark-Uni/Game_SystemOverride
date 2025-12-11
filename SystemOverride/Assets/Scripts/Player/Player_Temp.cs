using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Temp : MonoBehaviour
{
	private StateMachine<Player_Temp> _machine;
	[SerializeField] private GameObject _bulletPrefab;
	[SerializeField] private Transform _firePoint;
	private Vector2 _playerInput;
	Rigidbody2D _rb;
	Animator _am;

	public Vector2 playerInput
	{
		get { return _playerInput; }
	}
	PlayerInput _Input;
	public PlayerInput Input
	{
		get { return _Input; }
	}

	private int _facingDir;
	public int facingDir
	{
		get { return _facingDir; }
	}

	IdleState _idleState;
	public IdleState idleState
	{
		get { return _idleState; }
	}
	WalkState _walkState;
	public WalkState walkState
	{
		get { return _walkState; }
	}
	AttackState _attackState;
	public AttackState AttackState
	{
		get { return _attackState; }
	}

	public void SetAnimTrigger()
	{
		_machine.currentState.SetTrigger();
	}


	public void SetVelocity(float x, float y)
	{
		_rb.velocity = new Vector2(x, y);
		HandleFlip();
	}

	public void SetVelocity(in Vector2 force)
	{
		_rb.velocity = force;
		HandleFlip();
	}


	private void Flip()
	{
		transform.Rotate(0, 180, 0);
		_facingDir *= -1;
	}
	private void HandleFlip()
	{
		if (_playerInput.x == 1 && _facingDir == -1)
		{
			Flip();
			return;
		}
		if (_playerInput.x == -1 && _facingDir == 1)
		{
			Flip();
			return;
		}
	}

	private void Awake()
	{
		_Input = new PlayerInput();
		_machine = new StateMachine<Player_Temp>();
		_rb = GetComponent<Rigidbody2D>();
		_facingDir = 1;
	}

	void Start()
    {
		_am = GetComponentInChildren<Animator>();

		_idleState = new IdleState(this, _machine, "Idle", _rb, _am);
		_walkState = new WalkState(this, _machine, "Walk", _rb, _am);
		_attackState = new AttackState(this, _machine, "Attack", _rb, _am);

		_machine.BeginMachine(idleState);
	}

	private void OnEnable()
	{
		_Input.Enable();

		_Input.Player.Move.performed += ctx => _playerInput = ctx.ReadValue<Vector2>();
		_Input.Player.Move.canceled += ctx => _playerInput = Vector2.zero;
	}

	public void Shoot(out GameObject bullet)
	{
		bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
	}

	void Update()
    {
		_machine.currentState.EntityUpdate();
	}
}
