using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour //, IHitable
{

    // private StateMachine<Monster> _machine;
    public Rigidbody2D _Rb { get; private set; }
    public Animator _animator { get; private set; }

    // 몬스터 스탯
    private int _maxHp;
    private int _currentHp;

    public float _moveSpeed;
    public float _detectionRange;
    public float _attackDamage;

    // Patrol를 위한 변수설정
    public float _patrolRange = 5f;
    private Vector2 _startPosition;
    
    
    // 플레이어 위치 받기
    public Transform target;

   

    private void Awake()
    {
        _Rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        // _machine = new StateMachine<Monster>();
        /*
         * StateIdle = new IdleState(this, _machine)
         * StatePatrol = new PatrolState(this, _machine)
         * StateChase = new ChaseState(this, _machine)
         * StateAttack = new AttackState(this, _machine)
         */
    }

    private void Start()
    {
        _currentHp = _maxHp;
        _startPosition = transform.position;
        // 플레이어 태그 활용해서 존재하는지 체크
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null )
        {
            target = playerObj.transform;
            //_machine.Initialize(StateIdle); 초기상태 만들기
        }

    }

    private void Update()
    {
        // _machine.CurrentState.OnUpdate();
      

    }


    public void TakeDamage(int _damage)
    {
        // 이미 죽은 시체 또 때리는 거 방지
        if (_currentHp <= 0) return;

        _currentHp -= _damage;
        _animator.SetTrigger("Hit");

        if(_currentHp <= 0)
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
        _Rb.velocity = _direction * _moveSpeed;
        Flip(_direction.x);

    }
    // 방향 전환을 위한 함수
    public void Flip(float _xDirection)
    {
        Vector3 _currentScale = transform.localScale;

        // 오른쪽으로 이동
        if(_xDirection > 0.1f)
        {
            _currentScale.x = Mathf.Abs(_currentScale.x);
        }
        // 왼쪽으로 이동
        if(_xDirection < -0.1f)
        {
            _currentScale.x = - Mathf.Abs(_currentScale.x);
        }

        transform.localScale = _currentScale;
    }

    // 상태 변화를 위한 플레이어와 거리 체크
    public float GetToTarget()
    {
        if (target == null) return 9999f;
        return Vector2.Distance(transform.position, target.position);
    }
}
