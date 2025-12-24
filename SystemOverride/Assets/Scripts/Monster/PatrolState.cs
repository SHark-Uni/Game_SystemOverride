using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;


namespace Scripts.Monster
{
    public class PatrolState : MonsterSuperState
    {
        private float _patrolTimer;
        private float _currentPatrolTime;
        private float _patrolCenterX;
        protected Monster _monster;

        private float _footstepTimer;
        private float _footstepInterval = 0.4f; // 발소리 간격 (0.6초마다 재생)
        public PatrolState(Monster monster, StateMachine<Monster> stateMachine) : base(monster, stateMachine, "IsPatrol")
        {
            _monster = monster;
        }

        public override void Enter()
        {
            float _randomDir = Random.Range(0, 2) * 2 - 1;
            base.Enter();
            _patrolTimer = 0;
            _currentPatrolTime = Random.Range(_monster._patrolDuration * 0.7f, _monster._patrolDuration * 1.3f);
            _patrolCenterX = _monster.transform.position.x;
            _monster.Flip(_randomDir);
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
            bool _isDirectionChanged = false; // 방향이 바뀌었는지 체크하기 위한 변수
            if (_monster.IsCliff() || _monster.IsWall(_currentDir))
            {
                // 방향 즉시 반대로 전환
                _currentDir *= -1;
                _isDirectionChanged = true;
            }
            else
            {
                // 몬스터 정찰 범위 체크를 위한 변수 설정
                float _distFromStart = _monster.transform.position.x -_patrolCenterX;
                // 범위 벗어나면 방향 뒤집기 
                if (_distFromStart > _monster._patrolRange && _currentDir == 1)
                {
                    _currentDir = -1;
                    _isDirectionChanged = true;
                }
                else if (_distFromStart < -_monster._patrolRange && _currentDir == -1)
                {
                    _currentDir = 1;
                    _isDirectionChanged = true;
                }
            }
            

            if (_isDirectionChanged)
            {
                _monster.Flip(_currentDir);
            }

            //이동실행
            _monster.Move(new Vector2(_currentDir, 0));

            _footstepTimer += Time.deltaTime;
            if (_footstepTimer >= _footstepInterval)
            {
                // 소리 재생
                if (SoundManager.instance != null)
                    SoundManager.instance.PlaySFX("MonsterWalk", _monster.transform.position);

                // 타이머 리셋
                _footstepTimer = 0f;
            }


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






