using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Boss
{

    public class BossShockWave : MonoBehaviour, IAttacker
    {
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _lifeTime = 3.0f;

        public int attackPower => _damage;
        private void Start()
        {

            Destroy(gameObject, _lifeTime);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Boss")) return; 
           

            if (collision.CompareTag("Player"))
            {
                IDamageable target = collision.GetComponent<IDamageable>();
                if (target != null)
                {
                    {
                        target.TakeDamage(_damage, this);
                    }
                }
                Destroy(gameObject); // ¸ÂÃß¸é »ç¶óÁü
            }
            // º®¿¡ ´êÀ¸¸é »ç¶óÁü
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }
        public Vector3 GetAttackerPos()
        {
            return transform.position;
        }
        public void Attack(IDamageable target)
        {

        }
    }

}