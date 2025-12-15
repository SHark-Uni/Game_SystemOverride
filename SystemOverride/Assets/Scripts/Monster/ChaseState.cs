using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : MonsterSuperState
{
    public ChaseState(Monster monster, MonsterStateMachine stateMachine) : base(monster, stateMachine, "IsChase")
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        _monster._moveSpeed = _monster._chaseSpeed;
    }
    public override void OnUpdate()
    {
        if (_monster.GetToTarget() > _monster._detectionRange * 1.5f)
        {
            _stateMachine.ChangeState(_monster.StateIdle);
            return;
        }

        if (_monster.GetToTarget() <= _monster._attackRange)
        {
            _stateMachine.ChangeState(_monster.StateAttack);
            return; // 아래 이동 코드 실행 안 하고 종료
        }

        if (_monster._target != null)
        {
            float _direction = 0f;
            if (_monster._target.position.x > _monster.transform.position.x)
            {
                _direction = 1;
            }
            else
            {
                _direction = -1;
            }
            _monster.Move(new Vector2(_direction, 0));
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        _monster.Stop();
        _monster._moveSpeed = _monster._patrolSpeed;
    }
}
