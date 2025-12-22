using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.StateMachine;

namespace Scripts.Monster
{
    public class HitState : MonsterSuperState
    {
        private float _timer;
        protected Monster monster;
        public HitState(Monster _monster, StateMachine<Monster> _stateMachine) : base(_monster, _stateMachine, "IsHit")
        {
            monster = _monster;
        }

        public override void Enter()
        {
            base.Enter();
            _monster.Stop();
        }

        public override void EntityUpdate()
        {
            _timer += Time.deltaTime;

            if(_timer > _monster._hitRecoveryTime)
            {
                if(_monster._target != null && _monster.GetToTarget() <= _monster._detectionRange)
                {
                    _stateMachine.ChangeState(_monster.StateChase);
                }
                else
                {
                    _stateMachine.ChangeState(_monster.StateIdle);  
                }
            }
        }

        public override void Exit()
        {
        }
    }
}