using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;


namespace Scripts.Monster
{
    public class PatrolState : MonsterSuperState
    {
        private float _patrolTimer;
       
        public PatrolState(Monster monster, StateMachine<Monster> stateMachine) : base(monster, stateMachine, "IsPatrol")
        {
        }

        public override void Enter()
        {
            base.Enter();
            _patrolTimer = 0;
            _monster.Flip(Random.Range(0, 2) * 2 - 1);
        }

        public override void EntityUpdate()
        {
            // 추격 체크
            if (_monster.GetToTarget() < _monster._detectionRange)
            {
                _stateMachine.ChangeState(_monster.StateChase);
                return;
            }
            //낭떠러지 체크
            float _currentDir = Mathf.Sign(_monster.transform.localScale.x);
            if (_monster.IsCliff() || _monster.IsWall(_currentDir))
            {
                // 방향 즉시 반대로 전환
                _currentDir *= -1; // 방향 뒤집기 토글
            }
            // 이동 실행
            
            // 몬스터 정찰 범위 체크를 위한 변수 설정
            float _distFromStart = _monster.transform.position.x - _monster.GetStartPosition().x;
            // 범위 벗어나면 방향 뒤집기 
            if (Mathf.Abs(_distFromStart) > _monster._patrolRange)
            {
                if (_distFromStart > 0 && _currentDir == 1) _currentDir = -1;
                else if( _distFromStart < 0 && _currentDir == -1) _currentDir = 1;
            }
            //이동실행
            _monster.Move(new Vector2(_currentDir, 0));
            // 정찰 시간 설정
            _patrolTimer += Time.deltaTime;
            if (_patrolTimer > _monster._patrolDuration)
            {
                _stateMachine.ChangeState(_monster.StateIdle);
            }


        }
        public override void Exit()
        {
            base.Exit();
            _monster.Stop(); // 미끄러짐 방지
        }



    }
}






