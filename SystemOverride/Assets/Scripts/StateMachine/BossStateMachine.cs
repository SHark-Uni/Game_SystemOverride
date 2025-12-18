using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BossStateMachine
{
    public class BossStateMachine<T>
    {
        BossEntityState<T> _bosscurrentState;

        public BossEntityState<T> bosscurrentState
        {
            get { return _bosscurrentState; }
        }
        public void BeginMachine(BossEntityState<T> state)
        {
            _bosscurrentState = state;
            _bosscurrentState.Enter();
        }
        public void ChangeState(BossEntityState<T> state)
        {
            _bosscurrentState.Exit();
            _bosscurrentState = state;
            _bosscurrentState.Enter();
        }
    }
}