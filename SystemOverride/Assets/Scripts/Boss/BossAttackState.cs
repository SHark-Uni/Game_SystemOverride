using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Scripts.Common;

namespace Scripts.Boss
{
    public class BossAttackState : BossOnGroundState
    {
        bool _IsPlayVFX;
        public BossAttackState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
                : base(owner, stateMachine, name, rb, am)
        {
            name = "Attack";
        }

        void BossAttackPlayer(float bossAtk)
        {
            Debug.Log("BossAttackPlayer 함수 실행");
            // 보스와 플레이어의 콜라이더가 겹침
            //Collider2D _vshit = Physics2D.OverlapCircle(_bossOwner.transform.position, 2f, LayerMask.GetMask("Player"));

            //base.EntityUpdate();
            //Debug.Log("Attack 애니메이션 재생");
            if (_bossTrigger == true)
            {
                _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
            }
            Debug.Log("보스 공격 상태에서 플레이어와 콜라이더가 겹침, 5의 데미지");
        }

        public override void Enter()
        {
            base.Enter();
            _IsPlayVFX = false;
            SoundManager.instance.PlaySFX("BossAttack", _bossOwner.transform.position);
            _bossOwner.SetVelocity(Vector2.zero);
        }

        public override void EntityUpdate()
        {
            

            if (_bossTrigger)
            {
                _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public void CheckAttackHit()
        {
            Debug.Log("CheckAttackHit 실행됨");

            // 공격 범위 설정 
            // 보스 중심에서 보는 방향으로 1.0f 만큼 떨어진 곳을 타격 지점으로 잡음
            Vector2 attackPos = (Vector2)_bossOwner.transform.position + (Vector2.right * _bossOwner.bossfacingDir * 3.0f);
            float attackRadius = 2.5f; // 공격 범위 반지름

            // 범위 내의 플레이어 감지
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPos, attackRadius, LayerMask.GetMask("Player"));

            foreach (Collider2D player in hitPlayers)
            {
                IDamageable target = player.GetComponent<IDamageable>();
                if (target != null)
                {
                    
                    // IAttacker인 보스 본체에게 시킴
                    _bossOwner.Attack(target);

                    Debug.Log("플레이어에게 공격 적중!");
                }
            }
        }
    }
}
