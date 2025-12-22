using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;
using Scripts.Common;

namespace Scripts.Player
{
    public class PlayerOnGroundState : PlayerSuperState
    {
        // 대시 쿨타임 변수 생성
        const float _dashCoolTime = 1f;
        float _dashCoolStamp;
        // 백대시 쿨타임 변수 생성
        const float _backdashCoolTime = 1f;
        float _backdashCoolStamp;

        // Start is called before the first frame update
        public PlayerOnGroundState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
            // 대시 쿨타임 초기화
            _dashCoolStamp = _dashCoolTime;

            // 백대시 쿨타임 초기화
            _backdashCoolStamp = _backdashCoolTime;
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
            if (_inputAction.Attack.WasPerformedThisFrame())
            {
                if (CantAttack())
                {
                    return;
                }
                _stateMachine.ChangeState(_owner.attackState);
            }


            if (_inputAction.Jump.WasPerformedThisFrame())
            {
                _stateMachine.ChangeState(_owner.jumpState);
            }


            //GroundState인데, OnGround가 아니게됬다 -> 떨어지는중
            if (_owner.onGround == false)
            {
                _stateMachine.ChangeState(_owner.fallState);
            }

            // 대시 ChangeState 생성
            if (_dashCoolStamp > 0)
            {
                _dashCoolStamp -= Time.deltaTime;
            }
            else if (_dashCoolStamp < 0)
            {
                if (_inputAction.Dash.WasPerformedThisFrame())
                {
                    _stateMachine.ChangeState(_owner.dashState);

                    _dashCoolStamp = _dashCoolTime;
                }
            }
            // 백대시 ChangeState 생성
            if (_backdashCoolStamp > 0)
            {
                _backdashCoolStamp -= Time.deltaTime;
            }
            else if (_backdashCoolStamp < 0)
            {
                if (_inputAction.BackDash.WasPerformedThisFrame())
                {
                    _stateMachine.ChangeState(_owner.backdashState);

                    _backdashCoolStamp = _backdashCoolTime;
                }
            }

            if (_inputAction.SitDown.WasPerformedThisFrame())
            {
                _stateMachine.ChangeState(_owner.sitState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }


        private bool CantAttack()
        {
            return (_stateMachine.currentState == _owner.attackState);
        }
    }

}
