using Scripts.StateMachine;
using UnityEngine;

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
            monster.Stop();
            _timer = 0f;
        }

        public override void EntityUpdate()
        {
            _timer += Time.deltaTime;

            if (_timer > monster._hitRecoveryTime)
            {
                if (monster._target != null && monster.GetToTarget() <= monster._detectionRange)
                {
                    _stateMachine.ChangeState(monster.StateChase);
                }
                else
                {
                    _stateMachine.ChangeState(monster.StateIdle);
                }
            }
        }

        public override void Exit()
        {
        }
    }
}