using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;

namespace Scripts.Player
{
    public class HittedState : PlayerSuperState
    {
        float _timeStamp;
        public HittedState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am) 
            : base(owner, stateMachine, name, rb, am)
        {

        }

        public override void Enter()
        {
            base.Enter();
            _timeStamp = Time.time;
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            if (_timeStamp + _owner.hitDelay <= Time.time)
            {
                _stateMachine.ChangeState(_owner.idleState);
            }
        }


    }
}

