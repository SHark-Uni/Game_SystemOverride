using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonsterSuperState
{
    private float _idleTimer;
    private float _idleDuration = 1.5f;
    public IdleState(Monster monster, MonsterStateMachine stateMachine) : base(monster, stateMachine, "IsIdle")
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        _monster.Stop();
        _idleTimer = 0f;
    }

    public override void OnUpdate()
    {
        // 거리체크
        if (_monster.GetToTarget() < _monster._detectionRange)
        {
            _stateMachine.ChangeState(_monster.StateChase);
            return;
        }
        _idleTimer += Time.deltaTime;
        if (_idleTimer > _idleDuration)
        {
            _stateMachine.ChangeState(_monster.StatePatrol);
        }
    }


}
