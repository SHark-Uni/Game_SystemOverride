using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Scripts.StateMachine;
using System.ComponentModel;

namespace Scripts.Monster
{
    public class Monster : MonoBehaviour //, IHitable
    {
        //2종류.3종류,...50종류
        public MonsterStateMachine _machine { get; private set; }
        public Rigidbody2D _rb { get; private set; }
        public Animator _animator { get; private set; }
        public SpriteRenderer _spriteRenderer { get; private set; }
        // 상태
        public IdleState StateIdle { get; private set; }
        public PatrolState StatePatrol { get; private set; }
        public ChaseState StateChase { get; private set; }
        public AttackState StateAttack { get; private set; }

        // 몬스터 스탯
        private int _maxHp;
        private int _currentHp;

        public float _moveSpeed;
        public float _patrolSpeed;
        public float _chaseSpeed;

        public float _detectionRange;
        public float _attackDamage;

        //Idle 변수설정
        public float _idleWaitTime;

        // Patrol 변수설정
        public float _patrolRange;
        public float _patrolDuration;
        private Vector2 _startPosition;
        public Transform _cliffCheckPos;
        public eLayerMask _groundLayer;
        public float _cliffCheckDistance;
        public float _wallCheckDistance;
        // Attack 변수설정
        public float _attackRange; // 공격을 시작할 거리 
        public float _dashSpeed;// 공격 대쉬 속도
        public float _attackWaitTime = 0.5f;   // 공격 전 대기 시간 
        public float _dashDuration = 0.3f;       // 실제 돌진하는 시간
        public float _attackTotalTime = 1.5f;    // 전체 공격 모션 시간
        // 플레이어 위치 받기
        public Transform _target;
        public float _verticalDetectionRange;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _spriteRenderer = GetComponent<SpriteRenderer>();
            // 변수 설정
            _attackRange = 2f;
            _dashSpeed = 20f;
            _patrolSpeed = 2f;
            _patrolRange = 3f;
            _chaseSpeed = 4f;
            _detectionRange = 7.5f;
            _verticalDetectionRange = 1f;
            _wallCheckDistance = 0.6f;
            _cliffCheckDistance = 1f;
            _idleWaitTime = 1.5f;
            _patrolDuration = 3.0f;
        }

        private void Start()
        {
            _machine = new StateMachine<Monster>();

            StateIdle = new IdleState(this, _machine);
            StatePatrol = new PatrolState(this, _machine);
            StateChase = new ChaseState(this, _machine);
            StateAttack = new AttackState(this, _machine);
            _currentHp = _maxHp;
            _startPosition = transform.position;

            _moveSpeed = _patrolSpeed;
            // 플레이어 태그 활용해서 존재하는지 체크       
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _target = playerObj.transform;

            }
            _machine.BeginMachine(StateIdle);
        }

        private void Update()
        {
            if (_machine.currentState != null)
            {
                _machine.currentState.EntityUpdate();
            }


        }


        public void TakeDamage(int _damage)
        {
            // 이미 죽은 시체 또 때리는 거 방지
            if (_currentHp <= 0) return;

            _currentHp -= _damage;
            _animator.SetTrigger("Hit");

            if (_currentHp <= 0)
            {
                Die();
            }
            else
            {
                // 추격 상태로 만드는 로직
            }
        }


        public void Die()
        {
            _animator.SetBool("IsDead", true);
            // 콜라이더 끄고, 스크립트 비활성화하기
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;

            Destroy(gameObject, 1.5f);
        }

        // 몬스터 이동함수
        public void Move(Vector2 _direction)
        {
            _rb.velocity = _direction * _moveSpeed;
            Flip(_direction.x);

        }
        // 방향 전환을 위한 함수
        public void Flip(float _xDirection)
        {
            Vector3 _currentScale = transform.localScale;

            // 오른쪽으로 이동
            if (_xDirection > 0.1f)
            {
                _currentScale.x = Mathf.Abs(_currentScale.x);
            }
            // 왼쪽으로 이동
            if (_xDirection < -0.1f)
            {
                _currentScale.x = -Mathf.Abs(_currentScale.x);
            }

            transform.localScale = _currentScale;
        }
        // 몬스터가 멈추기 위한 함수
        public void Stop()
        {
            _rb.velocity = Vector2.zero;
        }

        public float GetToTarget()
        {

            if (_target == null) return 9999f;

            // y축 계산
            float _yDifference = Mathf.Abs(_target.position.y - transform.position.y);
            if (_yDifference > _verticalDetectionRange)
            {
                return 9999f;
            }

            //  X축 방향 계산
            // 오른쪽이면 (1, 0), 왼쪽이면 (-1, 0)
            float _dir = Mathf.Sign(_target.position.x - transform.position.x);
            Vector2 _xDirection = new Vector2(_dir, 0);
            // 레이어 마스크를 결합시켜 벽 뒤에 있는 플레이어를 감지 못하게 만듬
            int _layerMask = (int)(eLayerMask.Player | eLayerMask.Ground);
            // 수평 레이캐스트 발사
            RaycastHit2D _playerHit = Physics2D.Raycast(transform.position, _xDirection, _detectionRange, _layerMask);

            // 결과 판정      
            if (_playerHit.collider != null && _playerHit.collider.CompareTag("Player"))
            {
                // 실제 거리 반환 (x축 거리만 반환)
                return Mathf.Abs(_target.position.x - transform.position.x);
            }

            // 벽에 막혔거나 없으면 못 본 척
            return 9999f;
        }
        // 플레이어를 향해 특정 속도로 이동하기 위해 만든 함수
        public void MoveToTarget(float _speed)
        {
            if (_target == null) return;
            // 방향 구하기 (오른쪽은 1, 왼쪽은 -1)
            float _dir = Mathf.Sign(_target.position.x - transform.position.x);

            _rb.velocity = new Vector2(_dir * _speed, 0);
            Flip(_dir);
        }

        // 정찰 상태 범위 체크를 위해 
        public Vector2 GetStartPosition()
        {
            return _startPosition;
        }

        // 낭떠러지 체크
        public bool IsCliff()
        {
            RaycastHit2D _groundHit = Physics2D.Raycast(_cliffCheckPos.position, Vector2.down, _cliffCheckDistance, (int)_groundLayer);
            return _groundHit.collider == null;
        }
        // 벽 체크
        public bool IsWall(float _dir)
        {
            // 레이저 시작점
            Vector2 origin = transform.position;
            // 진행 방향(_dir)으로 레이저 발사      
            RaycastHit2D hit = Physics2D.Raycast(origin, new Vector2(_dir, 0), _wallCheckDistance, (int)_groundLayer);


            return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            // 눈높이 허용 범위
            Gizmos.color = Color.green;
            Vector3 center = transform.position;
            Vector3 size = new Vector3(_detectionRange * 2, _verticalDetectionRange * 2, 0);
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = Color.red;
            Vector3 origin = transform.position;
            Gizmos.DrawLine(origin, origin + Vector3.right * _wallCheckDistance);
            Gizmos.DrawLine(origin, origin + Vector3.left * _wallCheckDistance);

            // 실제 레이저 그리기 
            if (_target != null)
            {
                float yDiff = Mathf.Abs(_target.position.y - transform.position.y);
                if (yDiff <= _verticalDetectionRange)
                {
                    Vector2 xDir;


                    if (_target.position.x > transform.position.x)
                    {
                        xDir = Vector2.right; // (1, 0)
                    }
                    else
                    {
                        xDir = Vector2.left;  // (-1, 0)
                    }
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, transform.position + (Vector3)(xDir * _detectionRange));
                }
            }
            if (_cliffCheckPos != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(_cliffCheckPos.position, _cliffCheckPos.position + Vector3.down * _cliffCheckDistance);
            }
        }

    }
}

