using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Scripts.StateMachine;
using Scripts.Common;

namespace Scripts.Player
{
    public class BackDashState : PlayerOnGroundState
    {
        private SpriteRenderer _playerSpriteRenderer;
        private float _dashTimeStamp;
        private float _orginGravity;
        public BackDashState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
        }


        public override void Enter()
        {
            base.Enter();
            // 쿨타임 1초로 변경
            _owner._dashCooldown = 1f;

            _orginGravity = _rb.gravityScale;
            _dashTimeStamp = Time.time;
            
            _rb.gravityScale = 0;
            SoundManager.instance.PlaySFX("Dash", _owner.playerPosition);
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            _owner.SetVelocity(_owner._dashForce * -_owner.facingDir, 0);
            // 쿨타임이 0보다 클 경우 TIme.deltaTime으로 시간 감소해서 0까지 줄어들게 하는 조건
            if (_owner._dashCooldown > 0)
            {
                _owner._dashCooldown -= Time.deltaTime;

                if (_owner._dashCooldown < 0) _owner._dashCooldown = 0;
            }

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

