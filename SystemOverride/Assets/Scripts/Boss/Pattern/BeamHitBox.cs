using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Boss
{
    public class BeamHitBox : MonoBehaviour, IAttacker
    {
        [Header("Damage Details")]
        [SerializeField] private int _damage = 20;

      
        public int attackPower => _damage;

        public Vector3 GetAttackerPos()
        {
            // 넉백 방향 계산을 위해 빔의 위치 반환
            return transform.position;
        }

        public void Attack(IDamageable target)
        {
           
        }
       

       
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                IDamageable target = collision.GetComponent<IDamageable>();

                if (target != null)
                {
                    // 플레이어에게 데미지 전달 (자신을 공격자로 넘김)
                    target.TakeDamage(attackPower, this);
                }
            }
        }
    }
}