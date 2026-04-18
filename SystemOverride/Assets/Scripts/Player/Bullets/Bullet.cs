using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player.Bullets
{
    public class Bullet : MonoBehaviour, IPoolable, IAttacker
    {
        const float BULLET_ALIVE_TIME = 2.0f;
        public bool IsHackingBullet { get; private set; }
        public Material defaultMaterial;

        float lifeTime;
        Rigidbody2D rb;
        SpriteRenderer sr;

        public int attackPower
        {
            get { return 20; }
        }

        void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            sr = gameObject.GetComponent<SpriteRenderer>();
        }

        public void OnAlloc()
        {
            lifeTime = BULLET_ALIVE_TIME;
        }

        public void OnRelease()
        {
            rb.velocity = Vector2.zero;
            sr.material = defaultMaterial;
            IsHackingBullet = false;
        }

        void Update()
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                BulletManager.instance.DestroyBullet(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.CompareTag("Player")) return;

           
            IDamageable Target = collision.gameObject.GetComponent<IDamageable>();
            if (Target != null)
            {
                Attack(Target);
                BulletManager.instance.DestroyBullet(this);
                return; 
            }

           
       
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (collision.gameObject.layer == groundLayer)
            {
                BulletManager.instance.DestroyBullet(this);
            }
        }

        public Vector3 GetAttackerPos()
        {
            return transform.position;
        }

      
        public void SetHackingBullet()
        {
            IsHackingBullet = true;
        }

        public void SetNormalBullet()
        {
            IsHackingBullet = false;
        }

        public void Attack(IDamageable target)
        {
            target.TakeDamage(attackPower, this);
        }
    }
}