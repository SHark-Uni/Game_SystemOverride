using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Scripts.Player.Bullets
{
    public class Bullet : MonoBehaviour, IPoolable, IAttacker
    {
        const float BULLET_ALIVE_TIME = 2.0f;
        float _lifeTime;
        Rigidbody2D _rb;

        public int attackPower
        {
            get { return 20; }
        }

        public void OnAlloc()
        {
            _lifeTime = BULLET_ALIVE_TIME;
        }

        public void OnRelease()
        {
            _rb.velocity = Vector2.zero;
        }


        private void OnDestroy()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable Target = collision.gameObject.GetComponent<IDamageable>();
            if (Target != null)
            {
                Attack(Target);
            }
            BulletManager._instance.DestroyBullet(this);
        }

        void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
            
        }
       
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0)
            {
                BulletManager._instance.DestroyBullet(this);
            }
        }

        public Vector3 GetAttackerPos()
        {
            return transform.position;
        }

        public void Attack(IDamageable target)
        {
            target.TakeDamage(attackPower, this);
        }
    }
}

