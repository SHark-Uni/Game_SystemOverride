using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.StateMachine;

namespace Scripts.Player
{
    public class SitState : PlayerOnGroundState
    {
        private BoxCollider2D _boxCol;
        public SitState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
        }

        //¾É¾Æ¼­ °È´Â°Ô ÀÖ´Ù.
        // => 
        public override void Enter()
        {
            base.Enter();
            _owner.SitDown();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            OnSitDown();
        }

        public override void Exit()
        {
            base.Exit();

            _owner.StandUp();
        }

        void OnSitDown()
        {
            if (_inputAction.SitDown.WasReleasedThisFrame())
            {
                _stateMachine.ChangeState(_owner.idleState);
            }
        }



    }
}
