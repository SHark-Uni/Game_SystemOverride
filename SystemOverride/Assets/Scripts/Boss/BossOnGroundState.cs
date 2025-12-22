using Scipts.Boss;
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
        private BossStateMachine<Boss_Temp> _bossStateMachine;

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

        public void SetAnimTrigger()
        {
            _bossStateMachine.
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

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            // RayCast 시작 위치와 방향 설정
            Vector2 origin = (Vector2)_bossOwner.transform.position + Vector2.up * 1f;

            // RayCast 생성
            int layerMask = LayerMask.GetMask("Player");
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * _bossOwner.transform.localScale.x, 10f, layerMask);
            Debug.DrawRay(origin, Vector2.right * _bossOwner.transform.localScale.x * 10f, Color.yellow);

            // 보스와 플레이어의 콜라이더가 겹침
            Collider2D _vshit = Physics2D.OverlapCircle(_bossOwner.transform.position, 2f, LayerMask.GetMask("Player"));

            BossFlip();

            if (_vshit == null)
            {
                // 플레이어의 콜라이더와 겹치지 않았다면 보스 이동 상태로 전환
                _bossStateMachine.ChangeState(_bossOwner.bossWalkState);
            }
            // 보스와 플레이어의 콜라이더가 겹치고, 거리가 2 이내라면
            else if (_vshit != null && hit.distance <= 2f) 
            {
                // 공격 쿨타임 감소
                if (_bossattackCoolStamp > 0)
                {
                    _bossattackCoolStamp -= Time.deltaTime;
                }
                // 일반 공격 쿨타임 도달 시 보스 공격 상태로 전환
                else if (_bossattackCoolStamp < 0)
                {

                    _bossStateMachine.ChangeState(_bossOwner.bossAttackState);

                    _bossattackCoolStamp = _bossattackCoolTime;
                }
            }

            if(_bossOwner._bossHP <= 0)
            {
                _bossStateMachine.ChangeState(_bossOwner.bossDeathState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}


