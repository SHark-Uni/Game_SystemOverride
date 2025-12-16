using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;

namespace Scripts.Monster
{
    public class AttackState : MonsterSuperState
    {
        private float _timer;
        private bool _isDashing;

        public AttackState(Monster monster, StateMachine<Monster> stateMachine) 
            : base(monster, stateMachine, "IsAttack")
        {
        }


        public override void OnEnter()
        {
            base.OnEnter();
            _monster.Stop();
            _timer = 0f;
            _isDashing = false;
            // 몬스터 색을 빨갛게 바꿔서 공격 징조 보여주기
            _monster.GetComponent<SpriteRenderer>().color = Color.red;
        }

        public override void OnUpdate()
        {
            _timer += Time.deltaTime;

            // 0.5초 동안 기 모으기 (렉 걸린 척)
            if (_timer < 0.5f) return;

            // 대시 발사! (딱 한 번만 실행)
            if (!_isDashing)
            {
                _isDashing = true;
                // 플레이어 방향 구하기
                float dir;


                if (_monster._target.position.x > _monster.transform.position.x)
                {
                    dir = 1;  // 오른쪽 방향
                }
                else
                {
                    dir = -1; // 왼쪽 방향
                }
                // 대쉬
                _monster.Move(new Vector2(dir * _monster._dashSpeed, 0));
                _monster._rb.velocity = new Vector2(dir * _monster._dashSpeed, 0);
                _monster.Flip(dir);
            }

            // 대시 후 딜레이 (0.5초에 시작해서 0.3초간 돌진 후 멈춤)
            if (_timer > 0.8f)
            {
                _monster.Stop(); // 급정거
            }

            // 총 1.5초가 지나면 공격 끝, 상태 전환
            if (_timer > 1.5f)
            {
                // 플레이어가 여전히 가까우면 다시 공격, 아니면 추격
                if (_monster.GetToTarget() < _monster._attackRange)
                    _stateMachine.ChangeState(_monster.StateAttack); // 다시 공격 (루프)
                else
                    _stateMachine.ChangeState(_monster.StateChase); // 추격으로 복귀
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            // 색깔 원상복구
            _monster.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}

