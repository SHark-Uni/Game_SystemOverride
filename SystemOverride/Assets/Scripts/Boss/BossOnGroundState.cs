using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Scripts.Boss
{
    public class BossOnGroundState : BossSuperState
    {
        // 대시 쿨타임 변수 생성
        const float _bossdashCoolTime = 10f;
        float _dashCoolStamp;
        // 백대시 쿨타임 변수 생성
        const float _bossbackdashCoolTime = 15f;
        float _backdashCoolStamp;
        // 일반 공격 쿨타임 변수 생성
        const float _bossattackCoolTime = 3f;
        float _bossattackCoolStamp;
        // 공격 사거리 설정
        const float _attackRangeX = 2.3f;
        const float _attackRangeY = 1.0f;

        public BossOnGroundState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
            // 대시 쿨타임 초기화
            _dashCoolStamp = _bossdashCoolTime;
            // 백대시 쿨타임 초기화
            _backdashCoolStamp = _bossbackdashCoolTime;
            // 일반 공격 쿨타임 초기화
            _bossattackCoolStamp = _bossattackCoolTime;
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            if (_bossOwner._bossCurrentHp <= 0)
            {
                _bossStateMachine.ChangeState(_bossOwner.bossDeathState);
                return;
            }

            float hpRatio = (float)_bossOwner._bossCurrentHp / _bossOwner._bossMaxHp;

            // 33% 단위 체크
            if (hpRatio <= _bossOwner.nextLazerPatternThreshold)
            {
                // 다음 목표 갱신 (33% 깎기)
                _bossOwner.nextLazerPatternThreshold -= 0.33f;

                // 패턴 실행 후 바로 종료
                _bossStateMachine.ChangeState(_bossOwner._bossLazerAttackState);
                return;
            }

            // 바닥 패턴
            if (hpRatio <= _bossOwner.nextFloorPatternThreshold)
            {

                _bossOwner.nextFloorPatternThreshold -= 0.1f;


                _bossStateMachine.ChangeState(_bossOwner.bossFloorAttackState);
                return;
            }

            BossFlip();
            if (_bossattackCoolStamp > 0)
            {
                _bossattackCoolStamp -= Time.deltaTime;
            }

            // 거리 기반으로 수정
            // 플레이어와의 거리 계산
            float xDist = Mathf.Abs(_bossOwner.transform.position.x - _bossOwner._playerPos.position.x);

            // 세로 높이 차이 (절대값)
            float yDist = Mathf.Abs(_bossOwner.transform.position.y - _bossOwner._playerPos.position.y);

            // [조건] 가로가 사거리 안쪽이고(AND) && 높이 차이가 허용 범위 안쪽일 때만 공격
            bool canAttack = (xDist <= _attackRangeX) && (yDist <= _attackRangeY);

            if (canAttack)
            {
                // 사거리,높이 모두 충족하고, 쿨타임도 끝났으면 공격
                if (_bossattackCoolStamp <= 0)
                {
                    _bossStateMachine.ChangeState(_bossOwner.bossAttackState);
                    _bossattackCoolStamp = _bossattackCoolTime; // 쿨타임 리셋
                }
                else
                {
                    // 거리는 되는데 쿨타임 중이면 대기
                    _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
                }
            }
            else
            {
                // 가로가 멀거나, 혹은 너무 높이 있으면 추격
                _bossStateMachine.ChangeState(_bossOwner.bossWalkState);
            }
        }
        

        
        public void SetAnimTrigger()
        {
            _bossStateMachine.bosscurrentState.SetTrigger();
        }

        void BossFlip()
        {
            Vector2 scale = _bossOwner.transform.localScale;
            SpriteRenderer _bosssr = _bossOwner.GetComponent<SpriteRenderer>();

            // 보스가 항상 플레이어를 바라보게 설정
            if (_bossOwner._playerPos.position.x < _bossOwner.transform.position.x)
            {
                scale.x = -Mathf.Abs(scale.x);
                scale = new Vector3(scale.x, 1f, 1f);
                _bosssr.flipX = true;
            }
            else if (_bossOwner._playerPos.position.x > _bossOwner.transform.position.x)
            {
                scale.x = Mathf.Abs(scale.x);
                scale = new Vector3(scale.x, 1f, 1f);
                _bosssr.flipX = false;
            }
        }



        public override void Exit()
        {
            base.Exit();
        }
    }
}