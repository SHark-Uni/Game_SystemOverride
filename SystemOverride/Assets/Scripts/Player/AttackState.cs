using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Scripts.StateMachine;

namespace Scripts.Player
{
    public class AttackState : PlayerSuperState
    {
        float _preDelay;
        public AttackState(Player_Temp owner, StateMachine<Player_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _preDelay = _owner.preDelay;
            SpawnBullet();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
            _preDelay -= Time.deltaTime;

            //공격키 누르면 총알 나감.
            if (_preDelay <= 0)
            {
                _owner.SetVelocity(0, _rb.velocity.y);
            }

            if (_trigger == true)
            {
                _stateMachine.ChangeState(_owner.idleState);
            }
        }

        private void SpawnBullet()
        {
            GameObject bullet;
            _owner.Shoot(out bullet);

            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(15, 0) * _owner.facingDir, ForceMode2D.Impulse);
        }

    }
}

