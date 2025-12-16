using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Monster
{
    public class PatrolState : MonsterSuperState
    {
        private float _patrolTimer;
        private float _patrolDuration = 3.0f;
        private float _moveDirection = 1f;


        public PatrolState(Monster monster, MonsterStateMachine stateMachine) : base(monster, stateMachine, "IsPatrol")
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _patrolTimer = 0;
            int randomIndex = Random.Range(0, 2);

            if (randomIndex == 0)
            {
                _moveDirection = -1; // 0이 나오면 왼쪽(-1)
            }
            else
            {
                _moveDirection = 1;  // 1이 나오면 오른쪽(1)
            }
        }

        public override void OnUpdate()
        {
            // 추격 체크
            if (_monster.GetToTarget() < _monster._detectionRange)
            {
                _stateMachine.ChangeState(_monster.StateChase);
                return;
            }
            //낭떠러지 체크
            if (_monster.IsCliff() || _monster.IsWall(_moveDirection))
            {
                // 방향 즉시 반대로 전환
                if (_moveDirection == 1) _moveDirection = -1;
                else _moveDirection = 1;
            }
            // 이동 실행
            _monster.Move(new Vector2(_moveDirection, 0));
            // 몬스터 정찰 범위 체크를 위한 변수 설정
            float _distFromStart = _monster.transform.position.x - _monster.GetStartPosition().x;
            // 범위 벗어나면 방향 뒤집기 
            if (Mathf.Abs(_distFromStart) > _monster._patrolRange)
            {
                if (_distFromStart > 0) _moveDirection = -1;
                else _moveDirection = 1;
            }
            // 정찰 시간 설정
            _patrolTimer += Time.deltaTime;
            if (_patrolTimer > _patrolDuration)
            {
                _stateMachine.ChangeState(_monster.StateIdle);
            }


        }
        public override void OnExit()
        {
            base.OnExit();
            _monster.Stop(); // 미끄러짐 방지
        }



    }
}






