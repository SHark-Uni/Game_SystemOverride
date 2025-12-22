using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.BossStateMachine
{
    public abstract class BossEntityState<T>
    {
        protected T _bossOwner;
        protected BossStateMachine<T> _bossStateMachine;
        protected string _bossName;
        protected Rigidbody2D _bossRb;
        protected Animator _bossAm;

        protected bool _bossTrigger;
        protected float _bossDuration;

        public BossEntityState(T bossOwner, BossStateMachine<T> bossStateMachine, string bossName, Rigidbody2D bossRb, Animator bossAm)
        {
            _bossOwner = bossOwner;
            _bossStateMachine = bossStateMachine;
            _bossName = bossName;
            _bossRb = bossRb;
            _bossAm = bossAm;

            _bossTrigger = false;
            _bossDuration = 0;
        }

        public virtual void Enter()
        {
            _bossTrigger = false;
            _bossAm.SetBool(_bossName, true);
        }

        public virtual void EntityUpdate()
        {
        }

        public virtual void Exit()
        {
            _bossAm.SetBool(_bossName, false);
        }

        public void SetTrigger()
        {
            _bossTrigger = true;
        }
    }
}
