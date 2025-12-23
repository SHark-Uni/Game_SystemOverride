using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Scripts.StateMachine;
using System.ComponentModel;
using Scripts.Common;

namespace Scripts.Monster
{
    public class Monster : MonoBehaviour, IDamageable, IAttacker, Common.IPoolable
    {
        //2종류.3종류,...50종류
        public StateMachine<Monster> _machine { get; private set; }
        public Rigidbody2D _rb { get; private set; }
        public Animator _animator { get; private set; }
        public SpriteRenderer _spriteRenderer { get; private set; }
        // 상태
        public IdleState StateIdle { get; private set; }
        public PatrolState StatePatrol { get; private set; }
        public ChaseState StateChase { get; private set; }
        public AttackState StateAttack { get; private set; }
        public HitState StateHit { get; private set; }
        public int attackPower => (int)_attackDamage;

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
        private float _lastTouchDamageTime;
        // Hit 변수 설정
        public float _hitRecoveryTime;
        // 플레이어 위치 받기
        public Transform _target;
        public float _verticalDetectionRange;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _spriteRenderer = GetComponent<SpriteRenderer>();
            // 변수 설정
            Init();
        }

        private void Init()
        {
            _maxHp = 50;
            _attackRange = 3.5f;
            _dashSpeed = 20f;
            _patrolSpeed = 2f;
            _patrolRange = 3f;
            _chaseSpeed = 6f;
            _detectionRange = 7.5f;
            _verticalDetectionRange = 1f;
            _wallCheckDistance = 2f;
            _cliffCheckDistance = 0.3f;
            _idleWaitTime = 1.5f;
            _patrolDuration = 3.0f;
            _hitRecoveryTime = 0.5f;
            _attackDamage = 10f;
        }

        private void Start()
        {
            _machine = new StateMachine<Monster>();

            StateIdle = new IdleState(this, _machine);
            StatePatrol = new PatrolState(this, _machine);
            StateChase = new ChaseState(this, _machine);
            StateAttack = new AttackState(this, _machine);
            StateHit = new HitState(this, _machine);
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

            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("강제 피격 테스트!");
                // 데미지 10, 공격자는 null로 임시 테스트
                TakeDamage(10, null);
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
        private bool CanTouchAttack()
        {
            // (현재 시간 - 마지막 공격 시간)이 대기 시간보다 크면 공격 가능(True)
            return Time.time - _lastTouchDamageTime >= _attackWaitTime;
        }


        // 충돌처리 ( 몬스터 공격, 몸박공격 ) 
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (_machine.currentState != StateAttack) return;

                if (!CanTouchAttack())
                {
                    return;
                }

                IDamageable target = collision.gameObject.GetComponent<IDamageable>();

                if (target != null)
                {

                    Attack(target);


                }

            }
        }


        public void TakeDamage(int atk, IAttacker attacker)
        {

            if (_currentHp <= 0) return;

            _currentHp -= atk;
            _animator.SetTrigger("IsHit");

            if (_currentHp > 0)
            {
                _machine.ChangeState(StateHit);
            }
            else
            {
                Die();
            }
        }


        public void Attack(IDamageable target)
        {
            if (target != null)
            {
                target.TakeDamage(this.attackPower, this);
            }

        }

        public Vector3 GetAttackerPos()
        {
            return transform.position;
        }

        public void OnAlloc()
        {
            Init();
            _rb.velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = true;
            this.enabled = true;
        }

        public void OnRelease()
        {
            //물리정보 초기화.
            _rb.velocity = new Vector2(0, 0);
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

