using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Scripts.StateMachine;

namespace Scripts.Player
{
    public class DashState : PlayerOnGroundState
    {
        private SpriteRenderer _playerSpriteRenderer;

        public DashState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
        }
        private float _dashTimeStamp;
        private float _orginGravity;

        public override void Enter()
        {
            base.Enter();

            _orginGravity = _rb.gravityScale;
            _dashTimeStamp = Time.time;

            _owner.SetVelocity(_owner._dashForce * _owner.facingDir, _rb.velocity.y);
            _rb.gravityScale = 0;
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            if (_dashTimeStamp + _owner._dashDuration < Time.time)
            {
                _stateMachine.ChangeState(_owner.idleState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            _rb.gravityScale = _orginGravity;
        }
    }
}

