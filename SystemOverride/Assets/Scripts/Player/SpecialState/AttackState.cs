using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Scripts.Common;
using Scripts.Player.Bullets;
using Scripts.StateMachine;
using Scripts.Skill;

namespace Scripts.Player
{
    public class AttackState : PlayerSuperState
    {
        IDamageable _targetOrNull;
        bool _IsPlayVFX;
        float _preDelay;
        public AttackState(Player owner, StateMachine<Player> stateMachine, string name, Rigidbody2D rb, Animator am)
            : base(owner, stateMachine, name, rb, am)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _preDelay = _owner.preDelay;
            _IsPlayVFX = false;
            
            if (_owner.IsUsingSkill(eSkillBitMask.LaserBuster))
            {
                SoundManager._instance.PlaySFX("Laser", _owner.playerPosition);
                return;
            }

            SoundManager._instance.PlaySFX("Shoot", _owner.playerPosition);
            if (_owner.IsUsingSkill(eSkillBitMask.HackBullet))
            {
                SpawnBullet(eBulletType.Hacking);
                return;
            }
            SpawnBullet(eBulletType.Normal);
            return;
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
            _preDelay -= Time.deltaTime;

            if (_owner.IsUsingSkill(eSkillBitMask.LaserBuster))
            {
                ShootBlaster();
            }

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


        private void SpawnBullet(eBulletType bulletTypeId)
        {
            Bullet bullet;
            bullet = BulletManager.instance.CreatedBullet(_owner.firePosition, Quaternion.identity);
            Rigidbody2D createdRb = bullet.GetComponent<Rigidbody2D>();
            SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();

            switch (bulletTypeId)
            {
                case eBulletType.Normal:
                    bullet.SetNormalBuullet();
                    break;
                case eBulletType.Hacking:
                    //Material 바꾸기
                    sr.material = _owner._HackingBulletMaterial;
                    bullet.SetHakcingBullet();
                    //횟수 차감 하기
                    Buff buf = _owner.buffManager.FindCountBaseSkillOrNull((int)eSkillId.HackBullet);
                    buf.DecreaseCount();
                    break;
                default:
                    break;
            }

            bullet.gameObject.SetActive(true);
            createdRb.AddForce(_owner.attackForce * _owner.facingDir, ForceMode2D.Impulse);
        }

        private void ShootBlaster()
        {
            RaycastHit2D hit = Physics2D.Raycast(_owner.playerPosition, Vector2.right * _owner.facingDir, _owner.attackDistance, (int)eLayerMask.Monster);
            if (hit.collider == null)
            {
                _targetOrNull = null;
                return;
            }

            Debug.Log(hit.collider.gameObject.name);
            /*
            _targetOrNull = hit.collider.GetComponent<IDamageable>();
            _targetOrNull.TakeDamage(_owner.attackPower, _owner);*/

            //vfx연출
            if (_IsPlayVFX == false)
            {
                Vector2 up1 = hit.point;
                up1.y += 1.0f;
                Vector2 up2 = up1;
                up2.y += 1.0f;

                VFXManager._instance.PlayEffect(eVFXId.laserVFX, hit.point, Quaternion.identity);
                VFXManager._instance.PlayEffect(eVFXId.laserVFX, up1, Quaternion.identity);
                VFXManager._instance.PlayEffect(eVFXId.laserVFX, up2, Quaternion.identity);
                _IsPlayVFX = true;
            }
            
        }
    }
}

