using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.StateMachine;

namespace Scripts.Player
{
    public class SitState : PlayerOnGroundState
    {
        private CapsuleCollider2D _boxCol;
        public SitState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
        }

        //¾É¾Æ¼­ °È´Â°Ô ÀÖ´Ù.
        // => 
        public override void Enter()
        {
            base.Enter();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
            _owner.SitDown();
            OnSitDown();
        }

        public override void Exit()
        {
            base.Exit();
        }
        void OnSitDown()
        {
            if (_inputAction.SitDown.WasReleasedThisFrame())
            {
                _stateMachine.ChangeState(_owner.idleState);
                _owner.StandUp();
            }
        }
    }
}
