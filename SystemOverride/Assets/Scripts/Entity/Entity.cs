using Scripts.Common;
using Scripts.Player.Bullets;
using Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Scripts.Entity
{
    public class Entity : MonoBehaviour
    {
        [Header("Basic Collision Check")]
        public Transform _centerPos;
        public Vector2 _boxSize;

        protected bool _onGround;
        protected bool _hitWall;
        protected int _facingDir;

        protected Rigidbody2D _rb;
        protected Animator _animator;

        protected float _groundDistance;
        protected float _wallDistance;

        //SoundManager, VFX Manager는 따로 둘수도 있고, 그냥 공통적으로 들어있을 수도 있을듯.


        [Header("Basic Health")]
        protected int _maxHp;
        protected int _hp;
        
        public bool onGround
        {
            get { return _onGround; }
        }

        public int facingDir
        {
            get { return _facingDir; }
        }

        public int Hp
        {
            get { return _hp; }
            set
            {
                if (value <= 0)
                {
                    OnDie();
                    return;
                }
                _hp = value;
            }
        }
        public int MaxHp
        {
            get { return _maxHp; }
        }
        protected virtual void OnDie()
        { 
            
        }

        public void SetVelocity(float x, float y)
        {
            _rb.velocity = new Vector2(x, y);
            HandleFlip(x);
        }
        public void SetVelocity(in Vector2 force)
        {
            _rb.velocity = force;
            HandleFlip(force.x);
        }


        private void CheckOnCollision()
        {
            RaycastHit2D hit = Physics2D.BoxCast(_centerPos.position, _boxSize, 0f, Vector2.down, _groundDistance, (int)eLayerMask.Ground);
            _hitWall = Physics2D.Raycast(_centerPos.position, Vector2.right * _facingDir, _wallDistance, (int)eLayerMask.Ground);

            if (hit.collider != null)
            {
                _onGround = true;
            }
            else
            {
                _onGround = false;
            }
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _facingDir *= -1;
        }
        private void HandleFlip(float xVelocity)
        {
            if (xVelocity > 0 && _facingDir == -1)
            {
                Flip();
                return;
            }
            if (xVelocity < 0 && _facingDir == 1)
            {
                Flip();
                return;
            }
        }

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            CheckOnCollision();
            //_machine.currentState.EntityUpdate();
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(_centerPos.position, Vector2.down * _groundDistance, Color.black);
            Gizmos.DrawWireCube(_centerPos.position + Vector3.down * _groundDistance, _boxSize);
        }
    }


}
