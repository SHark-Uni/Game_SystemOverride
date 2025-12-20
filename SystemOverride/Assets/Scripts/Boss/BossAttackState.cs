using Scipts.Boss;
using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scripts.Boss
{
    public class BossAttackState : BossOnGroundState
    {
        public BossAttackState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
                : base(owner, stateMachine, name, rb, am)
        {
        }

        float BossAttackPlayer(float bossAtk)
        {
            Debug.Log("BossAttackPlayer 함수 실행");

            // 보스와 플레이어의 콜라이더가 겹침
            Collider2D _vshit = Physics2D.OverlapCircle(_bossOwner.transform.position, 2f, LayerMask.GetMask("Player"));

            //base.EntityUpdate();
            if (_vshit == null)
            {
                _bossAm.SetBool("Attack", true);
                _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
                Debug.Log("보스 공격 상태에서 플레이어와 콜라이더가 안 겹침, 정지 상태로 전환");
                _bossAm.SetBool("Attack", false);
                return bossAtk = 0;
            }
            else if (_vshit != null)
            {
                _bossAm.SetBool("Attack", true);
                _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
                Debug.Log("보스 공격 상태에서 플레이어와 콜라이더가 겹침, 5의 데미지");
                _bossAm.SetBool("Attack", false);
                return bossAtk;
            }
            return bossAtk;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            BossAttackPlayer(_bossOwner._bossAtk);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
