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
        public float _bossidleTime = 2;

        public BossIdleState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
            name = "Idle";
        }

        void Idle()
        {
            _bossidleTime -= Time.deltaTime;

            _bossAm.SetBool("Idle", true);

            if (_bossidleTime <= 0)
            {
                _bossStateMachine.ChangeState(_bossOwner.bossWalkState);
                _bossidleTime = 2;
            } 
        }

        public override void Enter()
        {
            base.Enter();
            _bossOwner.BossSetVelocity(0, _bossRb.velocity.y);
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            Idle();
        }

        public override void Exit()
        {
            _bossAm.SetBool("Idle", false);
            base.Exit();
        }
    }
}
