using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
            Collider2D _vshit = Physics2D.OverlapCircle(_bossOwner.transform.position, 2f, LayerMask.GetMask("Player"));

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
        }

        public override void EntityUpdate()
        {
            Debug.Log("BossAttackState EntityUpdate 실행");

            base.EntityUpdate();

            BossAttackPlayer(_bossOwner._bossAtk);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
