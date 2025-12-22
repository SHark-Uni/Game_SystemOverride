using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Scripts.StateMachine;


namespace Scripts.Monster
{
    public class ChaseState : MonsterSuperState
    {
        public ChaseState(Monster monster, StateMachine<Monster> stateMachine) 
            : base(monster, stateMachine, "IsChase")
        {
        }

        public override void Enter()
        {
            base.Enter();

            _monster._moveSpeed = _monster._chaseSpeed;
        }
        public override void EntityUpdate()
        {
            //내 감지범위에서 벗어나면, Idle로 바꾸기
            if (_monster.GetToTarget() > _monster._detectionRange * 1.5f)
            {
                _stateMachine.ChangeState(_monster.StateIdle);
                return;
            }

            //내 공격범위 안에 있다면, Attack으로 전환
            if (_monster.GetToTarget() <= _monster._attackRange)
            {
                _stateMachine.ChangeState(_monster.StateAttack);
                return; // 아래 이동 코드 실행 안 하고 종료
            }
            // 추격에 
            _monster.MoveToTarget(_monster._chaseSpeed);
        }

        public override void Exit()
        {
            base.Exit();
            _monster.Stop();
            _monster._moveSpeed = _monster._patrolSpeed;
        }
    }
}

