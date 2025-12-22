using Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Monster
{
    public class IdleState : MonsterSuperState
    {
        private float _idleTimer;
        protected Monster _monster;
        public IdleState(Monster monster, StateMachine<Monster> _stateMachine) : base(monster, _stateMachine, "IsIdle")
        {
            _monster = monster;
        }

        public override void Enter()
        {
            base.Enter();

            _monster.Stop();
            _idleTimer = 0f;
        }

        public override void EntityUpdate()
        {
            // 거리체크
            if (_monster.GetToTarget() < _monster._detectionRange)
            {
                _stateMachine.ChangeState(_monster.StateChase);
                return;
            }
            _idleTimer += Time.deltaTime;
            if (_idleTimer > _monster._idleWaitTime)
            {
                _stateMachine.ChangeState(_monster.StatePatrol);
            }
        }
        public override void Exit()
        {
            base.Exit();

        }


    }

}
