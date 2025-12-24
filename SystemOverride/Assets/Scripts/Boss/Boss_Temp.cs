using Scripts.BossStateMachine;
using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Boss
{
    public class Boss_Temp : MonoBehaviour
    {
        private BossStateMachine<Boss_Temp> _bossMachine;

        [SerializeField] private Transform _bossfirePoint;
        [SerializeField] private bool _bossonGround;
        public PhysicsMaterial2D _bosssloopyMaterial;
        public PhysicsMaterial2D _bossfrictionMaterial;
        public Transform _playerPos;

        public Transform BossCenterPos;
        public Vector2 BossBoxSize;
        public BoxCollider2D BossboxCol;
        public int _bossfacingDir;
        public bool IsGrounded => _bossonGround; // 외부에서 _bossonGround 값을 읽기 위한 변수

        public int _bossCurrentHp;
        public int _bossAtk;
        public int _bossDef;
        public int _bossMaxHp;
        

        [Header("Move Details")]
        [SerializeField] private Vector2 _bossInput;
        [SerializeField] private Vector2 _bossmoveSpeed;
        [SerializeField] private float _bossgroundDistance;
        public float _bossattackCooldown;

        [Header("Attack Details")]
        [SerializeField] private float _bosspreDelay;
        [SerializeField] private Vector2 _bossattackForce;


        [Header("Floor Attack Deatils")]
        public GameObject _floorAttackPrefab;
        public Transform _floorAttackSpawnPoint;
        public float _floorAttackDelay;
        public float _floorStateDuration;
        public float _floorPrefabLifeTime;
        public BossFloorAttackState StateFloorAttack => _bossFloorAttackState; // 외부에서 접근용 프로퍼티

        [Header("Laser Attack Deatils")]
        public GameObject _turretPrefab;   // 레이저 터렛 프리팹 넣는 곳
        public Transform[] _ceilingSpawnPoints; // 천장 스폰 위치들 배열

        [Header("Pattern Thresholds")]
        // FloorAttack: 10%씩 깎일 때마다 (시작은 90%부터 체크)
        public float nextFloorPatternThreshold = 0.9f;

        // Lazer: 33%씩 깎일 때마다 (시작은 66%부터 체크)
        public float nextLazerPatternThreshold = 0.66f;
       
        Rigidbody2D _bossrb;
        Animator _bossam;

        public Rigidbody2D BossRb => _bossrb;
        public Animator BossAnim => _bossam;

        // ?媛??ㅼ 蹂??
        private BossIdleState _bossidleState;
        private BossWalkState _bosswalkState;
        private BossFirstPatternState _bossFirstPatternState;
        private BossSecondPatternState _bossSecondPatternState;
        private BossDeathState _bossdeathState;
        private BossAttackState _bossattackState;
        private BossHitState _bosshitState;
        private BossFloorAttackState _bossFloorAttackState;
        public BossLazerAttackState _bossLazerAttackState;
        public Vector2 bossmoveSpeed { get { return _bossmoveSpeed; } }
        public float bosspreDelay { get { return _bosspreDelay; } }
        public Vector2 bossattackForce { get { return _bossattackForce; } }
        public int bossfacingDir { get { return _bossfacingDir; } }
        public BossIdleState bossIdleState { get { return _bossidleState; } }
        public BossWalkState bossWalkState { get { return _bosswalkState; } }
        public BossFirstPatternState bossFirstPatternState { get { return _bossFirstPatternState; } }
        public BossSecondPatternState bossSecondPatternState { get { return _bossSecondPatternState; } }
        public BossDeathState bossDeathState { get { return _bossdeathState; } }
        public BossAttackState bossAttackState { get { return _bossattackState; } }
        public BossHitState bossHitState { get { return _bosshitState; } }
        public BossFloorAttackState bossFloorAttackState { get { return _bossFloorAttackState; } }
        public void BossSetAnimTrigger()
        {
            _bossMachine.bosscurrentState.SetTrigger();
        }

        public void BossSetSoundTrigger()
        {
            _bossMachine.bosscurrentState.SetTrigger();
        }

        public void BossSetVelocity(float x, float y)
        {
            _bossrb.velocity = new Vector2(x, y);
        }

        public void SetVelocity(in Vector2 force)
        {
            _bossrb.velocity = force;
        }

        public void CheckOnGround()
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

        public void Init()
        {
            _bossMaxHp = 100;
            _bossAtk = 5;
            _bossDef = 5;
            _floorAttackDelay = 1f;
            _floorStateDuration = 0.5f;
            _floorPrefabLifeTime = 1.0f;
            _bossfacingDir = 1;
            _bosspreDelay = 0.4f;
            _bossCurrentHp = _bossMaxHp;
            
        }
        private void Awake()
        {
            Init();
            _bossMachine = new BossStateMachine<Boss_Temp>();
            _bossrb = GetComponent<Rigidbody2D>();
            _bossam = GetComponent<Animator>();
           
            BossboxCol = GetComponent<BoxCollider2D>();

            _bossmoveSpeed = new Vector2(4.0f, 0);

        
            _bossattackForce = new Vector2(15, 0f);

            BossBoxSize = new Vector2(0.35f, 0.3f);
        }

        void Start()
        {


            _bossidleState = new BossIdleState(this, _bossMachine, "Idle", _bossrb, _bossam);
            _bosswalkState = new BossWalkState(this, _bossMachine, "Move", _bossrb, _bossam);
            _bossFirstPatternState = new BossFirstPatternState(this, _bossMachine, "FirstPattern", _bossrb, _bossam);
            _bossSecondPatternState = new BossSecondPatternState(this, _bossMachine, "SecondPattern", _bossrb, _bossam);
            _bossdeathState = new BossDeathState(this, _bossMachine, "Death", _bossrb, _bossam);
            _bossattackState = new BossAttackState(this, _bossMachine, "Attack", _bossrb, _bossam);
            _bosshitState = new BossHitState(this, _bossMachine, "Hit", _bossrb, _bossam);
            _bossFloorAttackState = new BossFloorAttackState(this, _bossMachine, "Floor", _bossrb, _bossam);
            _bossLazerAttackState = new BossLazerAttackState(this, _bossMachine, _bossrb, _bossam);
            _bossMachine.BeginMachine(bossIdleState);
        }

        void Update()
        {
            CheckOnGround();

            _bossMachine.bosscurrentState.EntityUpdate();
            // 테스트용
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("바닥 찍기 패턴 설정");
                _bossMachine.ChangeState(StateFloorAttack);
            }
            // 테스트용
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("레이저 패턴 발동");
                _bossMachine.ChangeState(_bossLazerAttackState);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _bossCurrentHp -= 10;
                Debug.Log("보스 현재 Hp  : "  + _bossCurrentHp);
               
            }
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(BossCenterPos.position, Vector2.down * _bossgroundDistance, Color.black);
            Gizmos.DrawWireCube(BossCenterPos.position + Vector3.down * _bossgroundDistance, BossBoxSize);
        }
    }
}
