using Scipts.Boss;
using Scripts.Boss;
using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Boss
{
    public class BossIdleState : BossOnGroundState
    {
        public BossIdleState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _bossOwner.BossSetVelocity(0, _bossRb.velocity.y);
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            //키입력을 한다면, Walk 상태로 전파
            /* if (_bossOwner.playerInput.x != 0)
            {
                _bossStateMachine.ChangeState(_bossOwner.bossWalkState);
            } */
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
