using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Scripts.Skill;
using Scripts.Common;
using Scripts.Monster;
using Scripts.Player.Bullets;
using Scripts.StateMachine;
using Unity.VisualScripting;

namespace Scripts.Player
{
    public class Player : MonoBehaviour, IDamageable, IAttacker
    {
        private StateMachine<Player> _machine;
        
        [SerializeField] private Transform _firePoint;
        [SerializeField] private bool _onGround;

        public PhysicsMaterial2D _sloopyMaterial;

        public Material _invisibleMaterial;
        public Material _HackingBulletMaterial;
        public Material _ImmotalMaterial;
        public Material _DefaultMaterial;

        private SpriteRenderer _SpriteRender;
        Rigidbody2D _rb;
        Animator _am;

        public Transform CharacterCenterPos;
        public Vector2 BoxSize;
        // 앉기에서 콜라이더 적용을 위한 콜라이더 생성
        public CapsuleCollider2D capCol;

        [Header("Move Details")]
        [SerializeField] private Vector2 _playerInput;
        [SerializeField] private Vector2 _moveSpeed;
        [SerializeField][Range(0, 1)] private float _airMoveMulplier;

        public float _dashForce;
        public float _dashDuration;
        public float _dashCooldown;

        public int facingDir { get; private set; }

        [Header("Attack Details")]
        [SerializeField] private float _preDelay;
        [SerializeField] private Vector2 _attackForce;
        [SerializeField] private float _attackSpeed;
        public float attackDistance { get; private set; }

        [Header("Jump Details")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _groundDistance;
        [SerializeField] private float _slideDistance;
        [SerializeField] private bool _doubleJump;

        [Header("SitDown Deatils")]
        [SerializeField] private Vector2 _sitDownColiderCapSize;
        [SerializeField] private Vector2 _sitDownColiderOffset;
        [SerializeField] private Vector2 _sitDownUpColiderCapSize;
        [SerializeField] private Vector2 _sitDownUpColiderOffset;

        [Header("Hitted Delay")]
        [SerializeField] private float _hitDelay;

        [Header("Player Stats")]
        [SerializeField] private PlayerStat _playerStat;
        public ulong _skillAction;

        public PlayerSkillComponent skillHandler { get; private set; }
        public BuffManager buffManager { get; private set; }
        public Vector2 _clickedPoint { get; private set; }
        public LineRenderer rope { get; private set; }
        public float grappleLength { get; private set; }
        public DistanceJoint2D joint { get; private set; }

        PlayerInput _Input;

        // 앉기 전용 콜라이더 생성
        public CapsuleCollider2D _capCol;

        public IdleState idleState { get; private set; }
        public WalkState walkState { get; private set; }
        public AttackState attackState { get; private set; }
        public FallState fallState { get; private set; }
        public JumpState jumpState { get; private set; }
        public JumpAttackState jumpAttackState { get; private set; }
        // 대시, 백대시, 앉기 변수 생성
        public DashState dashState { get; private set; }
        public BackDashState backdashState { get; private set; }
        public SitState sitState { get; private set; }
        public HittedState hittedState { get; private set; }
        public GrappleState grappleState { get; private set; }

        public Vector3 playerPosition
        {
            get { return transform.position; }
        }
        public Vector2 currentMousePosition
        {
            get { return _clickedPoint; }
        }

        public float hitDelay
        {
            get { return _hitDelay; }
        }
        public Vector3 firePosition
        {
            get { return _firePoint.position; }
        }
        public Vector2 sitDownColiderCapSize
        {
            get { return _sitDownColiderCapSize; }
        }
        public Vector2 sitDownColiderOffset
        {
            get { return _sitDownColiderOffset; }
        }

        public Vector2 sitDownUpColiderCapSize
        {
            get { return _sitDownUpColiderCapSize; }
        }
        public Vector2 sitDownUpColiderOffset
        {
            get { return _sitDownUpColiderOffset; }
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

        public int attackPower
        {
            get { return _playerStat._atk; }
        }

        private bool SetHp(int value)
        {
            if (value <= 0)
            {
                OnDie();
                return true;
            }
            _playerStat._hp = value;
            return false;
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

        public void SetVelocityNoFlipCharcter(float x, float y)
        {
            _rb.velocity = new Vector2(x, y);
        }

        private void CheckOnGround()
        {
            RaycastHit2D hit = Physics2D.BoxCast(CharacterCenterPos.position, BoxSize, 0f, Vector2.down, _groundDistance, (int)eLayerMask.Ground | (int)eLayerMask.Hook);
            if (hit.collider != null)
            {
                _onGround = true;
            }
            else
            {
                _onGround = false;
            }

        }

        public void Flip()
        {
            transform.Rotate(0, 180, 0);
            facingDir *= -1;
        }
        private void HandleFlip()
        {
            if (_playerInput.x == 1 && facingDir == -1)
            {
                Flip();
                return;
            }
            if (_playerInput.x == -1 && facingDir == 1)
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
            _capCol.size = _sitDownColiderCapSize;
            _capCol.offset = _sitDownColiderOffset;
        }

        public void StandUp()
        {
            _capCol.size = _sitDownUpColiderCapSize;
            _capCol.offset = _sitDownUpColiderOffset;
        }

        private void Awake()
        {
            _Input = new PlayerInput();
            _machine = new StateMachine<Player>();
            _rb = GetComponent<Rigidbody2D>();
            _capCol = GetComponent<CapsuleCollider2D>();

            buffManager = new BuffManager(this);

            _playerStat = new PlayerStat(100, 5, 5);

            facingDir = 1;
            _airMoveMulplier = .8f;
            _moveSpeed = new Vector2(6.0f, 0);
            attackDistance = 10.0f;

            _preDelay = 0.1f;
            _hitDelay = 0.25f;

            _attackSpeed = 1f;
            _attackForce = new Vector2(20, 0f);

            _doubleJump = true;
            _jumpForce = 15.0f;

            _dashForce = 20f;
            _dashDuration = 0.25f;
            _dashCooldown = 0f;

            _groundDistance = 0.85f;
            grappleLength = 2.0f;

            BoxSize = new Vector2(0.35f, 0.05f);

            _sitDownColiderCapSize = new Vector2(0.7f, 0.1f);
            _sitDownColiderOffset = new Vector2(0.08f, -0.1f);

            _sitDownUpColiderCapSize = new Vector2(0.7f, 1.3f);
            _sitDownUpColiderOffset = new Vector2(0.08f, -0.1f);
        }

        void Start()
        {
            _am = GetComponentInChildren<Animator>();
            joint = gameObject.GetComponent<DistanceJoint2D>();
            rope = gameObject.GetComponentInChildren<LineRenderer>();
            _SpriteRender = gameObject.GetComponentInChildren<SpriteRenderer>();

            joint.enabled = false;
            rope.enabled = false;

            skillHandler = new PlayerSkillComponent(this);
            InitStates();

            _machine.BeginMachine(idleState);
        }

        private void OnEnable()
        {
            _Input.Enable();

            _Input.Player.Move.performed += ctx => _playerInput = ctx.ReadValue<Vector2>();
            _Input.Player.Move.canceled += ctx => _playerInput = Vector2.zero;

            _Input.Player.Look.performed += ctx => _clickedPoint = ctx.ReadValue<Vector2>();


            //_Input.Player.Hook.canceled += ctx => ClickedPoint = Vector2.zero;
        }

        void Update()
        {
            CheckOnGround();
            HandleSkill();

            _machine.currentState.EntityUpdate();
            buffManager.UpdateBuff();
        }
        private void OnDrawGizmos()
        {
            Debug.DrawRay(CharacterCenterPos.position, Vector2.down * _groundDistance, Color.black);
            Debug.DrawRay(CharacterCenterPos.position, Vector2.right * facingDir * attackDistance, Color.black);
            Gizmos.DrawWireCube(CharacterCenterPos.position + Vector3.down * _groundDistance, BoxSize);
        }




        public void TakeDamage(int atk, IAttacker attacker)
        {
            Debug.Log("Player TakeDamage 호출됨");
            //무적이면 무시
            if (IsUsingSkill(eSkillBitMask.Immotal)) 
            {
                Debug.Log("Immotal?? 통과");
                return;
            }

            bool IsDead;
            //데미지 계산
            IsDead = SetHp(atk);
            if (IsDead)
            {
                return;
            }

            //연출
            Vector3 dir = (attacker.GetAttackerPos() - CharacterCenterPos.position).normalized;
            dir.y = 0;

            //피격음 연출
            SoundManager.instance.PlaySFX("Hitted", transform.position);

            //피격 VFX 연출
            Vector3 spawnPos = CharacterCenterPos.position + dir;
            VFXManager.instance.PlayEffect(eVFXId.onHitVFX, spawnPos, Quaternion.identity);
            SetVelocity(dir.x * -1, _rb.velocity.y);

            //피격상태로 전환
            _machine.ChangeState(hittedState);
        }

        private void HandleSkill()
        {
            //SkillTest
            if (_Input.Player.SKill_HackingBullet.WasPerformedThisFrame())
            {
                skillHandler.prepareHackBullet();
            }

            if (_Input.Player.Skill_Invisible.WasPerformedThisFrame())
            {
                skillHandler.InvisibleSkillProcess();
            }

            if (_Input.Player.Skill_Immotal.WasPerformedThisFrame())
            {
                //5초간 무적상태로 만들기, 쿨다운 90초
                skillHandler.ImmotalSKillProcess();
            }

            if (_Input.Player.Skill_Blaster.WasPerformedThisFrame())
            {
                //20초 동안, 쿨다운 1분
                //공격시 레이저를 발사하는 공격으로 변환
                skillHandler.LaserBlasterProcess();
            }
        }
        private void OnDie()
        {
            SceneLoader.instance.LoadScene(eSceneType._Ending);
        }

        public bool IsUsingSkill(eSkillBitMask mask)
        {
            return (_skillAction & (ulong)mask) != 0;
        }

        public void SetUseSkill(eSkillBitMask mask)
        {
            _skillAction |= (ulong)mask;
        }

        public void SetUnuseSkill(eSkillBitMask mask)
        {
            _skillAction &= ~(ulong)mask;
        }


        private void InitStates()
        {
            idleState = new IdleState(this, _machine, "Idle", _rb, _am);
            walkState = new WalkState(this, _machine, "Walk", _rb, _am);
            attackState = new AttackState(this, _machine, "Attack", _rb, _am);
            fallState = new FallState(this, _machine, "Jump/Fall", _rb, _am);
            jumpState = new JumpState(this, _machine, "Jump/Fall", _rb, _am);
            jumpAttackState = new JumpAttackState(this, _machine, "JumpAttack", _rb, _am);
            // 앉기 애니메니션 생성자
            sitState = new SitState(this, _machine, "SitDown", _rb, _am);
            dashState = new DashState(this, _machine, "Dash", _rb, _am);
            backdashState = new BackDashState(this, _machine, "BackDash", _rb, _am);
            hittedState = new HittedState(this, _machine, "Hitted", _rb, _am);
            grappleState = new GrappleState(this, _machine, "Grappled", _rb, _am);
        }


        public Vector3 GetAttackerPos()
        {
            return transform.position;
        }
        public void Attack(IDamageable target)
        {

        }

        public void ChangeMaterial(Material material)
        {
            _SpriteRender.material = material;
        }

        public void ResetMaterial()
        {
            _SpriteRender.material = _DefaultMaterial;
        }
    }

}
