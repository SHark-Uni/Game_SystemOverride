using Scipts.Boss;
using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public BossOnGroundState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
            // 대시 쿨타임 초기화
            _dashCoolStamp = _bossdashCoolTime;
            // 백대시 쿨타임 초기화
            _backdashCoolStamp = _bossbackdashCoolTime;
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
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


