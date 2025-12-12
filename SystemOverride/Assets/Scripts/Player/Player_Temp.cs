using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Temp : MonoBehaviour
{
	private StateMachine<Player_Temp> _machine;
	[SerializeField] private GameObject _bulletPrefab;
	[SerializeField] private Transform _firePoint;
	[SerializeField] private bool _onGround;

	[Header("Move Details")]
	[SerializeField] private Vector2 _playerInput;
	[SerializeField] private Vector2 _moveSpeed;
	[SerializeField] [Range(0,1)] private float _airMoveMulplier;
	private int _facingDir;

	[Header("Attack Details")]
	[SerializeField] private float _preDelay;
	[SerializeField] private Vector2 _attackForce;
	[SerializeField] private float _attackSpeed;

	[Header("Jump Details")]
	[SerializeField] private float _jumpForce;
	[SerializeField] private float _groundDistance;

	Rigidbody2D _rb;
	Animator _am;
	PlayerInput _Input;
	
	private IdleState _idleState;
	private WalkState _walkState;
	private AttackState _attackState;
	private FallState _fallState;
	private JumpState _jumpState;

	public Vector2 moveSpeed
	{
		get { return _moveSpeed; }
	}
	public float airMoveMultiplier
	{
		get { return _airMoveMulplier; }
	}
	public bool onGround
	{
		get { return _onGround; }
	}
	public float jumpForce
	{
		get { return _jumpForce; }
	}
	public float preDelay
	{
		get { return _preDelay; }
	}
	public Vector2 attackForce
	{
		get { return _attackForce; }
	}
	public float attackSpeed
	{
		get { return _attackSpeed; }
	}
	public Vector2 playerInput
	{
		get { return _playerInput; }
	}
	public PlayerInput Input
	{
		get { return _Input; }
	}
	public int facingDir
	{
		get { return _facingDir; }
	}
	public IdleState idleState
	{
		get { return _idleState; }
	}
	public WalkState walkState
	{
		get { return _walkState; }
	}
	public AttackState AttackState
	{
		get { return _attackState; }
	}
	public JumpState jumpState
	{
		get { return _jumpState; }
	}
	public FallState fallState
	{
		get { return _fallState; }
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


	private void CheckOnGround()
	{
		_onGround = Physics2D.Raycast(transform.position, Vector2.down, _groundDistance, (int)eLayerMask.Ground);
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

		_airMoveMulplier = .8f;

		_moveSpeed = new Vector2(6.0f, 0);

		_preDelay = 0.1f;
		_attackSpeed = 1f;
		_attackForce = new Vector2(20, 0f);

		_jumpForce = 15.0f;
		_groundDistance = 1.1f;
	}

	void Start()
    {
		_am = GetComponentInChildren<Animator>();

		_idleState = new IdleState(this, _machine, "Idle", _rb, _am);
		_walkState = new WalkState(this, _machine, "Walk", _rb, _am);
		_attackState = new AttackState(this, _machine, "Attack", _rb, _am);
		_fallState = new FallState(this, _machine, "Jump/Fall", _rb, _am);
		_jumpState = new JumpState(this, _machine, "Jump/Fall", _rb, _am);


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
		CheckOnGround();
		_machine.currentState.EntityUpdate();
	}

	private void OnDrawGizmos()
	{
		Debug.DrawRay(transform.position, Vector2.down * _groundDistance, Color.black);
	}
}
