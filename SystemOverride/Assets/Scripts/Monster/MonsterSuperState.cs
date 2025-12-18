using Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Monster
{
    public abstract class MonsterSuperState : EntityState<Monster>
    {
        protected Monster _monster;
        private StateMachine<Monster> _stateMachine;
        private string v;

        public MonsterSuperState(Monster monster, StateMachine<Monster> stateMachine, string animationName) 
            : base(monster, stateMachine, animationName, monster._rb, monster._animator)
        {
        }

        /*protected MonsterSuperState(Monster monster, StateMachine<Monster> stateMachine, string v)
            : base(monster, stateMachine, animationName, monster._rb, monster._animator)
        {
            _monster = monster;
            this._stateMachine = stateMachine;
            this.v = v;
        }*/
    }

}
