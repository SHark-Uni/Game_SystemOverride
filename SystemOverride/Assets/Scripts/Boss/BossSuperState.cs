using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Boss
{
    public class BossSuperState : BossEntityState<Boss_Temp>
    {
    public BossSuperState(Boss_Temp boss_owner, BossStateMachine<Boss_Temp> _bossstateMachine, string name, Rigidbody2D _bossrb, Animator _bossam)
        : base(boss_owner, _bossstateMachine, name, _bossrb, _bossam)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
            //_bossAm.SetFloat("yVelocity", _bossRb.velocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

