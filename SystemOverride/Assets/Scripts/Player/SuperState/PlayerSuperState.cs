using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Scripts.StateMachine;

namespace Scripts.Player
{
    public class PlayerSuperState : EntityState<Player>
    {
        protected PlayerInput.PlayerActions _inputAction;
        // 대시, 백대시, 앉기 상태용 New Input System 생성

        public PlayerSuperState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
            _inputAction = _owner.Input.Player;

        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
            _am.SetFloat("yVelocity", _rb.velocity.y);

            //TEST
            if (_inputAction.HookKeyboard.WasPerformedThisFrame())
            {
                _stateMachine.ChangeState(_owner.grappleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }

}
