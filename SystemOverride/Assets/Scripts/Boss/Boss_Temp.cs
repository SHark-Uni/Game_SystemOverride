using Scripts.Boss;
using Scripts.BossStateMachine;
using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scipts.Boss
{
    public class Boss_Temp : MonoBehaviour
    {
        private BossStateMachine<Boss_Temp> _bossMachine;
        //오브젝트 풀이 필요하다면 여기

        [SerializeField] private Transform _bossfirePoint;
        [SerializeField] private bool _bossonGround;
        public PhysicsMaterial2D _bosssloopyMaterial;
        public PhysicsMaterial2D _bossfrictionMaterial;

        public Transform BossCenterPos;
        public Vector2 BossBoxSize;
        public BoxCollider2D BossboxCol;

        [Header("Move Details")]
        [SerializeField] private Vector2 _bossInput;
        [SerializeField] private Vector2 _bossmoveSpeed;
        [SerializeField][Range(0, 1)] private float _bossairMoveMulplier;
        public float _bossdashForce;
        public float _bossdashDuration;
        public float _bossdashCooldown;
        private int _bossfacingDir;

        [Header("Attack Details")]
        [SerializeField] private float _bosspreDelay;
        [SerializeField] private Vector2 _bossattackForce;
        [SerializeField] private float _bossattackSpeed;

        [Header("Jump Details")]
        [SerializeField] private float _bossjumpForce;
        [SerializeField] private float _bossgroundDistance;
        [SerializeField] private float _bossslideDistance;
        [SerializeField] private bool _bossdoubleJump;

        [Header("SitDown Deatils")]
        [SerializeField] private Vector2 _bosssitDownColiderBoxSize;
        [SerializeField] private Vector2 _bosssitDownColiderOffset;
        [SerializeField] private Vector2 _bosssitDownUpColiderBoxSize;
        [SerializeField] private Vector2 _bosssitDownUpColiderOffset;

        Rigidbody2D _bossrb;
        Animator _bossam;

        // 상태값 설정 변수
        private BossIdleState _bossidleState;
        private BossWalkState _bosswalkState;
        private BossDashState _bossdashState;
        private BossBackDashState _bossbadkdashState;
        private BossJumpState _bossjumpState;
        private BossFirstPatternState _bossFirstPatternState;
        private BossSecondPatternState _bossSecondPatternState;
        private BossDeathState _bossdeathState;
        private BossAttackState _bossattackState;
        private BossFallState _bossfallState;
        private BossHitState _bosshitState;
        private BossSitState _bosssitState;

        public Vector2 bosssitDownColiderBoxSize { get { return _bosssitDownColiderBoxSize; } }
        public Vector2 bosssitDownColiderOffset { get { return _bosssitDownColiderOffset; } }
        public Vector2 bosssitDownUpColiderBoxSize { get { return _bosssitDownUpColiderBoxSize; } }
        public Vector2 bosssitDownUpColiderOffset { get { return _bosssitDownUpColiderOffset; } }
        public Vector2 bossmoveSpeed { get { return _bossmoveSpeed; } }
        public float bossairMoveMulplier { get { return _bossairMoveMulplier; } }
        public float bossjumpforce { get { return _bossjumpForce; } }
        public float bosspreDelay { get { return _bosspreDelay; } }
        public Vector2 bossattackForce { get { return _bossattackForce; } }
        public float bossattackSpeed { get { return _bossattackSpeed; } }
        public int bossfacingDir { get { return _bossfacingDir; } }
        public BossIdleState bossIdleState { get { return _bossidleState; } }
        public BossWalkState bossWalkState { get { return _bosswalkState; } }
        public BossDashState bossDashState { get { return _bossdashState; } }
        public BossBackDashState bossBackDashState { get { return _bossbadkdashState; } }
        public BossJumpState bossJumpState { get { return _bossjumpState; } }
        public BossFirstPatternState bossFirstPatternState { get { return _bossFirstPatternState; } }
        public BossSecondPatternState bossSecondPatternState { get { return _bossSecondPatternState; } }
        public BossDeathState bossDeathState { get { return _bossdeathState; } }
        public BossAttackState bossAttackState { get { return _bossattackState; } }
        public BossFallState bossFallState { get { return _bossfallState; } }
        public BossHitState bossHitState { get { return _bosshitState; } }
        public BossSitState bossSitState { get { return _bosssitState; } }

        public void BossSetAnimTrigger()
        {
            _bossMachine.bosscurrentState.SetTrigger();
        }

        public void BossSetVelocity(float x, float y)
        {
            _bossrb.velocity = new Vector2(x, y);
            HandleFlip();
        }

        public void SetVelocity(in Vector2 force)
        {
            _bossrb.velocity = force;
            HandleFlip();
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _bossfacingDir *= -1;
        }
        private void HandleFlip()
        { // 추후에 입력
            /*
            if (_playerInput.x == 1 && _bossfacingDir == -1)
            {
                Flip();
                return;
            }
            if (_playerInput.x == -1 && _bossfacingDir == 1)
            {
                Flip();
                return;
            }
            */
        }

        private void CheckOnGround()
        {
            RaycastHit2D hit = Physics2D.BoxCast(BossCenterPos.position, BossBoxSize, 0f, Vector2.down, _bossgroundDistance, (int)eLayerMask.Ground);
            if (hit.collider != null)
            {
                _bossonGround = true;
            }
            else
            {
                _bossonGround = false;
            }
        }

        public void BossSitDown()
        {
            BossboxCol.size = _bosssitDownColiderBoxSize;
            BossboxCol.offset = _bosssitDownColiderOffset;
        }

        public void BossStandUp()
        {
            BossboxCol.size = _bosssitDownUpColiderBoxSize;
            BossboxCol.offset = _bosssitDownUpColiderOffset;
        }

        private void Awake()
        {
            _bossMachine = new BossStateMachine<Boss_Temp>();
            _bossrb = GetComponent<Rigidbody2D>();
            _bossfacingDir = 1;
            BossboxCol = GetComponent<BoxCollider2D>();
            _bossairMoveMulplier = .8f;

            _bossmoveSpeed = new Vector2(4.0f, 0);

            _bosspreDelay = 0.4f;
            _bossattackSpeed = 1f;
            _bossattackForce = new Vector2(15, 0f);

            _bossjumpForce = 12f;
            _bossgroundDistance = 1f;
            _bossdashForce = 5f;
            _bossdashDuration = 0.2f;
            _bossdashCooldown = 10f;

            BossBoxSize = new Vector2(0.35f, 0.3f);

            // 보스 앉기의 콜라이더, offset 크기 설정
            _bosssitDownColiderBoxSize = new Vector2(0.7f, 0.2f);
            _bosssitDownColiderOffset = new Vector2(0f, -0.15f);
            // 보스 일어나기의 콜라이더, offset 크기 설정
            _bosssitDownUpColiderBoxSize = new Vector2(0.7f, 0.35f);
            _bosssitDownUpColiderOffset = new Vector2(0f, 0f);
        }

        void Start()
        {
            _bossam = GetComponent<Animator>();

            _bossidleState = new BossIdleState(this, _bossMachine, "BossIdle", _bossrb, _bossam);
            _bosswalkState = new BossWalkState(this, _bossMachine, "BossWalk", _bossrb, _bossam);
            _bossdashState = new BossDashState(this, _bossMachine, "BossDash", _bossrb, _bossam);
            _bossbadkdashState = new BossBackDashState(this, _bossMachine, "BossBackDash", _bossrb, _bossam);
            _bossjumpState = new BossJumpState(this, _bossMachine, "BossJump", _bossrb, _bossam);
            _bossFirstPatternState = new BossFirstPatternState(this, _bossMachine, "BossFirstPattern", _bossrb, _bossam);
            _bossSecondPatternState = new BossSecondPatternState(this, _bossMachine, "BossSecondPattern", _bossrb, _bossam);
            _bossdeathState = new BossDeathState(this, _bossMachine, "BossDeath", _bossrb, _bossam);
            _bossattackState = new BossAttackState(this, _bossMachine, "BossAttack", _bossrb, _bossam);
            _bossfallState = new BossFallState(this, _bossMachine, "BossFall", _bossrb, _bossam);
            _bosshitState = new BossHitState(this, _bossMachine, "BossHit", _bossrb, _bossam);
            _bosssitState = new BossSitState(this, _bossMachine, "BossSit", _bossrb, _bossam);

            _bossMachine.BeginMachine(bossIdleState);
        }

        void Update()
        {
            CheckOnGround();
            _bossMachine.bosscurrentState.EntityUpdate();
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(BossCenterPos.position, Vector2.down * _bossgroundDistance, Color.black);
            Gizmos.DrawWireCube(BossCenterPos.position + Vector3.down * _bossgroundDistance, BossBoxSize);
        }
    }
}
