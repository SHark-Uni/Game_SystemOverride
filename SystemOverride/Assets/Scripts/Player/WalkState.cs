using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;

namespace Scripts.Player
{
    public class WalkState : PlayerOnGroundState
    {
        public WalkState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
        }

        // Start is called before the first frame update
        public override void Enter()
        {
            base.Enter();
        }
        public override void EntityUpdate()
        {
            base.EntityUpdate();

            //move를 하던중, 경사로 때문에, 갑자기 땅에서 떨어지는 경우

            if (_owner.playerInput.x == 0)
            {
                _stateMachine.ChangeState(_owner.idleState);
            }
            _owner.SetVelocity(_owner.playerInput.x * _owner.moveSpeed.x, _rb.velocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

}
