using Scipts.Boss;
using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Boss
{
    public class BossAttackState : BossOnGroundState
    {
        public BossAttackState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
                : base(owner, stateMachine, name, rb, am)
        {

        }

        void AttackPlayer()
        {
            _bossAm.SetBool("Attack", true);

            Collider2D hit = Physics2D.OverlapCircle(_bossOwner.transform.position, 2f, LayerMask.GetMask("Player"));

            if (hit == null)
            {
                Debug.Log("플레이어 회피");
                // 여기에 플레이어에게 데미지를 주는 코드를 추가하세요.
                return;
            }
            else
            {
                Debug.Log("플레이어 공격 성공");
                // 여기에 플레이어에게 데미지를 주는 코드를 추가하세요.
                return;
            }
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("보스 공격 상태 진입");
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            AttackPlayer();
        }

        public override void Exit()
        {
            _bossAm.SetBool("Idle", true);
            base.Exit();
        }
    }
}
