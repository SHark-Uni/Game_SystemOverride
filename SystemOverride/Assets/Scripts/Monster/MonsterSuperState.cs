using Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Monster
{
    public abstract class MonsterSuperState : EntityState<Monster>
    {
        protected Monster _monster;
        private MonsterStateMachine stateMachine;
        private string v;

        public MonsterSuperState(Monster monster, StateMachine<Monster> stateMachine, string animationName) : base(monster, stateMachine, animationName, monster._rb, monster._animator)
        {
        }

        protected MonsterSuperState(Monster monster, MonsterStateMachine stateMachine, string v)
        {
            _monster = monster;
            this.stateMachine = stateMachine;
            this.v = v;
        }
    }

}
