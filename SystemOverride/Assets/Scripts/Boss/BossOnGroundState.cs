using Scipts.Boss;
using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
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

            // RayCast 시작 위치와 방향 설정
            Vector2 origin = (Vector2)_bossOwner.transform.position + Vector2.up * 1f;
            int layerMask = LayerMask.GetMask("Player");
            Vector3 scale = _bossOwner.transform.localScale;

            // RayCast 생성
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * _bossOwner.transform.localScale.x, 10f, layerMask);
            Debug.Log(hit.collider); 
            Debug.DrawRay(origin, Vector2.right * _bossOwner.transform.localScale.x * 10f, Color.yellow);

            // 보스가 항상 플레이어를 바라보게 설정
            if (_bossOwner._playerPos.position.x < _bossOwner.transform.position.x)
            {
                scale.x = -Mathf.Abs(scale.x);
                _bossOwner.transform.localScale = new Vector3(scale.x, 1f, 1f);
            }
            else
            {
                scale.x = Mathf.Abs(scale.x);
                _bossOwner.transform.localScale = new Vector3(scale.x, 1f, 1f);
            }
            // RayCast가 플레이어에 맞으면 보스 이동 상태로 전환
            if (hit.collider != null && hit.collider.CompareTag("Player") && hit.distance > 2f)
            {
                Debug.Log("플레이어 인지");
                _bossStateMachine.ChangeState(_bossOwner.bossWalkState);
            }
            // 플레이어가 일정 거리 이내로 접근 시 보스 공격 상태로 전환
            if (hit.distance <= 2f)
            {
                // 대시 공격 쿨타임 감소
                if (_bossattackCoolStamp > 0) _bossattackCoolStamp -= Time.deltaTime;
                else if(_bossattackCoolStamp < 0) // 일반 공격 쿨타임 도달 시 보스 공격 상태로 전환
                {
                    Debug.Log("보스 공격 상태로 전환");
                    _bossStateMachine.ChangeState(_bossOwner.bossAttackState);
                    _bossattackCoolStamp = _bossattackCoolTime;
                }
                
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private bool BossCanAttack()
        {
            return (_bossStateMachine.bosscurrentState == _bossOwner.bossAttackState);
        }
    }
}


