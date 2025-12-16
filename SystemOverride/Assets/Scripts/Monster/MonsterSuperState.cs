using Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Monster
{
    public abstract class MonsterSuperState : EntityState<Monster>
    {
        protected Monster _monster;
       

        public MonsterSuperState(Monster monster, StateMachine<Monster> stateMachine, string animationName) : base(monster, stateMachine, animationName, monster._rb, monster._animator)
        {
        }


    }

}
