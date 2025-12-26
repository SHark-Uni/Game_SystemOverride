using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Scripts.StateMachine;
using Scripts.Common;

namespace Scripts.Monster
{
    public class ChaseState : MonsterSuperState
    {
        protected Monster _monster;
        private float _soundTimer;
        private float _soundInterval = 0.4f;
        public ChaseState(Monster monster, StateMachine<Monster> stateMachine) 
            : base(monster, stateMachine, "IsChase")
        {
            _monster = monster;
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
            // 추격
            _monster.MoveToTarget(_monster._chaseSpeed);

            _soundTimer += Time.deltaTime;

            if (_soundTimer >= _soundInterval)
            {
                // "MonsterRun" 이나 "MonsterWarning" 같은 사운드 이름 사용
                if (SoundManager.instance != null)
                {
                    SoundManager.instance.PlaySFX("MonsterChase", _monster.transform.position);
                }

                _soundTimer = 0f;
            }
        }

        public override void Exit()
        {
            base.Exit();
            _monster.Stop();
            _monster._moveSpeed = _monster._patrolSpeed;
        }
    }
}

