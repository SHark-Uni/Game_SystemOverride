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
        //?§Î∏å?ùÌä∏ ?Ä???ÑÏöî?òÎã§Î©??¨Í∏∞

        [SerializeField] private Transform _bossfirePoint;
        [SerializeField] private bool _bossonGround;
        public PhysicsMaterial2D _bosssloopyMaterial;
        public PhysicsMaterial2D _bossfrictionMaterial;
        public Transform _playerPos;

        public Transform BossCenterPos;
        public Vector2 BossBoxSize;
        public BoxCollider2D BossboxCol;
        public int _bossfacingDir;
        public bool IsGrounded => _bossonGround; // ø‹∫Œø°º≠ _bossonGround ∞™¿ª ¿–±‚ ¿ß«— ∫Øºˆ

        public int _bossHP = 100;
        public int _bossAtk = 5;
        public int _bossDef = 5;

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
        public float _floorAttackDelay = 0.5f;     
        public float _floorStateDuration = 0.5f;   
        public float _floorPrefabLifeTime = 1.0f;     
        public BossFloorAttackState StateFloorAttack => _bossFloorAttackState; // ø‹∫Œø°º≠ ¡¢±ŸøÎ «¡∑Œ∆€∆º

        [Header("Laser Attack Deatils")]
        public GameObject _turretPrefab;   // ∑π¿Ã¿˙ ≈Õ∑ø «¡∏Æ∆’ ≥÷¥¬ ∞˜
        public Transform[] _ceilingSpawnPoints; // √µ¿Â Ω∫∆˘ ¿ßƒ°µÈ πËø≠

        Rigidbody2D _bossrb;
        Animator _bossam;

        public Rigidbody2D BossRb => _bossrb;
        public Animator BossAnim => _bossam;

        // ?ÅÌÉúÍ∞??§Ï†ï Î≥Ä??
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
        public BossFloorAttackState bossFloorAttackState { get { return _bossFloorAttackState;} }
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

        private void Awake()
        {
            _bossMachine = new BossStateMachine<Boss_Temp>();
            _bossrb = GetComponent<Rigidbody2D>();
            _bossam = GetComponent<Animator>();
            _bossfacingDir = 1;
            BossboxCol = GetComponent<BoxCollider2D>();

            _bossmoveSpeed = new Vector2(4.0f, 0);

            _bosspreDelay = 0.4f;
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
            // ≈◊Ω∫∆ÆøÎ
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("πŸ¥⁄ ¬Ô±‚ ∆–≈œ º≥¡§");
                _bossMachine.ChangeState(StateFloorAttack);
            }
            // ≈◊Ω∫∆ÆøÎ
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("∑π¿Ã¿˙ ∆–≈œ πﬂµø");             
                _bossMachine.ChangeState(_bossLazerAttackState);
            }
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(BossCenterPos.position, Vector2.down * _bossgroundDistance, Color.black);
            Gizmos.DrawWireCube(BossCenterPos.position + Vector3.down * _bossgroundDistance, BossBoxSize);
        }
    }
}
