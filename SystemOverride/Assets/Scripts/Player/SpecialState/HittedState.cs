using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;
using Scripts.Common;

namespace Scripts.Player
{
    public class HittedState : PlayerSuperState
    {
        float _timeStamp;
        public HittedState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am) 
            : base(owner, stateMachine, name, rb, am)
        {

        }

        public override void Enter()
        {
            base.Enter();
            _timeStamp = Time.time;

            _owner.SetUseSkill(eSkillBitMask.Immotal);
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            if (_timeStamp + _owner.hitDelay <= Time.time)
            {
                _stateMachine.ChangeState(_owner.idleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _owner.SetUnuseSkill(eSkillBitMask.Immotal);
        }

    }
}

