using Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public class GrappleState : PlayerSuperState
    {
        public GrappleState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am) 
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
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

