using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;

namespace Scripts.Player
{
    public class PlayerOnGroundState : PlayerSuperState
    {
        // Start is called before the first frame update
        public PlayerOnGroundState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
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
                _stateMachine.ChangeState(_owner.AttackState);
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

            // 대시, 백대시, 앉기 ChangeState 생성
            if (_inputAction.Dash.WasPerformedThisFrame())
            {
                _stateMachine.ChangeState(_owner.dashState);
            }
            if (_inputAction.BackDash.WasPerformedThisFrame())
            {

                _stateMachine.ChangeState(_owner.backdashState);
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
            return (_stateMachine.currentState == _owner.AttackState);
        }
    }

}
