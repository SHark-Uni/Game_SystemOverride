using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;

namespace Scripts.Player
{
    public class FallState : PlayerAirState
    {
        public FallState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            if (_owner.onGround)
            {
                _stateMachine.ChangeState(_owner.idleState);
                _owner.AvailableDoubleJump();
            }
        }

    }
}

