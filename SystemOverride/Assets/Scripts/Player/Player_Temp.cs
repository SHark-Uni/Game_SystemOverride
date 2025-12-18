using System.Collections;
using System.Collections.Generic;
using Scripts.Player;
using UnityEngine;
using Scripts.StateMachine;
using Scripts.Monster;
using Scripts.Player.Bullets;
using Scripts.Common;

namespace Scripts.Player
{
    public class Player_Temp : MonoBehaviour, IDamageable
    {
        private StateMachine<Player_Temp> _machine;

        [SerializeField] private Transform _firePoint;
        [SerializeField] private bool _onGround;
        public PhysicsMaterial2D _sloopyMaterial;
        public PhysicsMaterial2D _frictionMaterial;

        public Transform CharacterCenterPos;
        public Vector2 BoxSize;
        // 앉기에서 콜라이더 적용을 위한 콜라이더 생성
        public BoxCollider2D boxCol;

        [Header("Move Details")]
        [SerializeField] private Vector2 _playerInput;
        [SerializeField] private Vector2 _moveSpeed;
        [SerializeField][Range(0, 1)] private float _airMoveMulplier;
        public float _dashForce;
        public float _dashDuration;
        public float _dashCooldown;
        private int _facingDir;

        [Header("Attack Details")]
        [SerializeField] private float _preDelay;
        [SerializeField] private Vector2 _attackForce;
        [SerializeField] private float _attackSpeed;

        [Header("Jump Details")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _groundDistance;
        [SerializeField] private float _slideDistance;
        [SerializeField] private bool _doubleJump;

        [Header("SitDown Deatils")]
        [SerializeField] private Vector2 _sitDownColiderBoxSize;
        [SerializeField] private Vector2 _sitDownColiderOffset;
        [SerializeField] private Vector2 _sitDownUpColiderBoxSize;
        [SerializeField] private Vector2 _sitDownUpColiderOffset;

        [Header("Hitted Delay")]
        [SerializeField] private float _hitDelay;

        [Header("Grappling Detail")]
        [SerializeField] private float grappleLength;
        [SerializeField] private int grappleLayer;
        [SerializeField] private Vector2 ClickedPoint;
        [SerializeField] private LineRenderer rope;

        private Vector3 grapplePoint;
        private DistanceJoint2D joint;


        Rigidbody2D _rb;
        Animator _am;
        PlayerInput _Input;
        // 대시, 백대시, 앉기 전용 New Input System 생성
        PlayerMove _playerMove;
        // 앉기 전용 콜라이더 생성
        public BoxCollider2D _boxCol;

        private IdleState _idleState;
        private WalkState _walkState;
        private AttackState _attackState;
        private FallState _fallState;
        private JumpState _jumpState;
        private JumpAttackState _jumpAttackState;
        // 대시, 백대시, 앉기 변수 생성
        private DashState _dashState;
        private BackDashState _backdashState;
        private SitState _sitState;
        private HittedState _hittedState;
        private GrappleState _grappleState;
        public float hitDelay
        {
            get { return _hitDelay; }
        }
        public Vector3 firePosition
        {
            get { return _firePoint.position; }
        }
        public Vector2 sitDownColiderBoxSize
        {
            get { return _sitDownColiderBoxSize; }
        }
        public Vector2 sitDownColiderOffset
        {
            get { return _sitDownColiderOffset; }
        }

        public Vector2 sitDownUpColiderBoxSize
        {
            get { return _sitDownUpColiderBoxSize; }
        }
        public Vector2 sitDownUpColiderOffset
        {
            get { return _sitDownUpColiderOffset; }
        }


        public JumpAttackState jumpAttackState
        {
            get { return _jumpAttackState; }
        }
        public Vector2 moveSpeed
        {
            get { return _moveSpeed; }
        }
        public float airMoveMultiplier
        {
            get { return _airMoveMulplier; }
        }
        public bool doubleJump
        {
            get { return _doubleJump; }
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
        // 대시, 백대시, 앉기 사용을 위한 New Input System 적용
        public PlayerMove playerMove
        {
            get { return _playerMove; }
        }
        public DashState dashState
        {
            get { return _dashState; }
        }
        public BackDashState backdashState
        {
            get { return _backdashState; }
        }
        public SitState sitState
        {
            get { return _sitState; }
        }

        public GrappleState grappleState
        {
            get { return _grappleState; }
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
            RaycastHit2D hit = Physics2D.BoxCast(CharacterCenterPos.position, BoxSize, 0f, Vector2.down, _groundDistance, (int)eLayerMask.Ground);
            if (hit.collider != null)
            {
                _onGround = true;
            }
            else
            {
                _onGround = false;
            }

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

        public void PressedDoubleJump()
        {
            _doubleJump = false;
        }

        public void AvailableDoubleJump()
        {
            _doubleJump = true;

        }

        public void SitDown()
        {
            _boxCol.size = _sitDownColiderBoxSize;
            _boxCol.offset = _sitDownColiderOffset;
        }

        public void StandUp()
        {
            _boxCol.size = _sitDownUpColiderBoxSize;
            _boxCol.offset = _sitDownUpColiderOffset;
        }

        private void Awake()
        {
            _Input = new PlayerInput();
            _machine = new StateMachine<Player_Temp>();
            _rb = GetComponent<Rigidbody2D>();
            _facingDir = 1;
            _boxCol = GetComponent<BoxCollider2D>();
            _airMoveMulplier = .8f;

            _moveSpeed = new Vector2(6.0f, 0);

            _preDelay = 0.1f;
            _attackSpeed = 1f;
            _attackForce = new Vector2(20, 0f);

            _doubleJump = true;

            _jumpForce = 15.0f;
            _groundDistance = 0.85f;
            _dashForce = 7f;
            _dashDuration = 0.2f;
            _dashCooldown = 0f;
            _hitDelay = 0.25f;

            grappleLayer = (int)eLayerMask.Ground;
            grappleLength = 2.0f;


            BoxSize = new Vector2(0.35f, 0.05f);

            _sitDownColiderBoxSize = new Vector2(0.7f, 0.1f);
            _sitDownColiderOffset = new Vector2(0.08f, -0.1f);

            _sitDownUpColiderBoxSize = new Vector2(0.7f, 1.3f);
            _sitDownUpColiderOffset = new Vector2(0.08f, -0.1f);
        }

        void Start()
        {
            _am = GetComponentInChildren<Animator>();
            joint = gameObject.GetComponent<DistanceJoint2D>();
            rope = gameObject.GetComponentInChildren<LineRenderer>();


            joint.enabled = false;
            rope.enabled = false;

            _idleState = new IdleState(this, _machine, "Idle", _rb, _am);
            _walkState = new WalkState(this, _machine, "Walk", _rb, _am);
            _attackState = new AttackState(this, _machine, "Attack", _rb, _am);
            _fallState = new FallState(this, _machine, "Jump/Fall", _rb, _am);
            _jumpState = new JumpState(this, _machine, "Jump/Fall", _rb, _am);
            _jumpAttackState = new JumpAttackState(this, _machine, "JumpAttack", _rb, _am);
            // 앉기 애니메니션 생성자
            _sitState = new SitState(this, _machine, "SitDown", _rb, _am);
            _dashState = new DashState(this, _machine, "Dash", _rb, _am);
            _backdashState = new BackDashState(this, _machine, "BackDash", _rb, _am);
            _hittedState = new HittedState(this, _machine, "Hitted", _rb, _am);
            _grappleState = new GrappleState(this, _machine, "Grapped", _rb, _am);

            _machine.BeginMachine(idleState);
        }

        private void OnEnable()
        {
            _Input.Enable();

            _Input.Player.Move.performed += ctx => _playerInput = ctx.ReadValue<Vector2>();
            _Input.Player.Move.canceled += ctx => _playerInput = Vector2.zero;

            _Input.Player.Look.performed += ctx => ClickedPoint = ctx.ReadValue<Vector2>();


            //_Input.Player.Hook.canceled += ctx => ClickedPoint = Vector2.zero;
        }



        void Update()
        {
            CheckOnGround();
            _machine.currentState.EntityUpdate();
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(CharacterCenterPos.position, Vector2.down * _groundDistance, Color.black);
            Gizmos.DrawWireCube(CharacterCenterPos.position + Vector3.down * _groundDistance, BoxSize);

        }

        public bool TryHook()
        {
            Vector3 worldpos;
            Vector3 renderPos = new Vector3(ClickedPoint.x, ClickedPoint.y, -Camera.main.transform.position.z);

            worldpos = Camera.main.ScreenToWorldPoint(renderPos);

            RaycastHit2D hit = Physics2D.Raycast(
               origin: worldpos,
               direction: Vector2.zero,
               distance: Mathf.Infinity,
               layerMask: grappleLayer);

            if (hit.collider != null)
            {
                grapplePoint = hit.point;
                grapplePoint.z = 0;

                joint.connectedAnchor = grapplePoint;
                joint.enabled = true;
                joint.distance = grappleLength;

                rope.SetPosition(0, grapplePoint);
                rope.SetPosition(1, transform.position);

                rope.enabled = true;
                return true;
            }
            return false;
        }

        public void TakeDamage(int atk, IAttacker attacker)
        {
            Vector3 dir = (attacker.GetAttackerPos() - CharacterCenterPos.position).normalized;
            dir.y = 0;

            Vector3 spawnPos = CharacterCenterPos.position + dir;
            VFXManager.instance.PlayEffect(eVFXId.onHitVFX, spawnPos, Quaternion.identity);
            SetVelocity(dir.x * -1, _rb.velocity.y);

            //피격상태로 전환
            _machine.ChangeState(_hittedState);
        }
    }

}
