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
            name = "Attack";
        }

        float AttackPlayer(float _bossAtk)
        {
            _bossAm.SetBool("Attack", true);

            // 보스와 플레이어의 콜라이더가 겹침
            Collider2D hit = Physics2D.OverlapCircle(_bossOwner.transform.position, 2f, LayerMask.GetMask("Player"));

            // 이후 보스 공격 애니메이션 실행


            if (hit == null)
            {
                Debug.Log("플레이어 회피");

                // 플레이어가 회피를 하여 데미지를 주지 못해, 0으로 리턴
                _bossAtk = 0;
                
                return _bossAtk;
            }
            else
            {
                Debug.Log("플레이어 공격 성공");

                // 플레이어에게 데미지를 주어 보스 공격력을 리턴
                Debug.Log(_bossAtk + "만큼 데미지 입힘");
                return _bossAtk;
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

            AttackPlayer(_bossOwner._bossAtk);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
