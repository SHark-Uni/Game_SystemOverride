using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Player.Bullets;
using Scripts.StateMachine;
using Scripts.Common;

namespace Scripts.Player
{
    public class JumpAttackState : PlayerAirState
    {
        float _preDelay;

        public JumpAttackState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am)
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
                if (_owner.onGround == false)
                {
                    _stateMachine.ChangeState(_owner.fallState);
                }
                else
                {
                    _stateMachine.ChangeState(_owner.idleState);
                }
            }
        }

        private void SpawnBullet()
        {
            Bullet bullet;
            bullet = BulletManager.instance.CreatedBullet(_owner.firePosition, Quaternion.identity);

            bullet.gameObject.SetActive(true);

            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(15, 0) * _owner.facingDir, ForceMode2D.Impulse);
        }

    }
}

