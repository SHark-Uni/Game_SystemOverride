using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Scripts.Common;

namespace Scripts.Boss
{
    public class FallDebris : MonoBehaviour, IAttacker
    {
        [Header("낙하 설정")]
        public float _warningDuration = 0.3f; // 떨어지기 전 대기 시간 
        public float _fallGravity = 0.5f;     // 떨어질 때 중력 (속도)
        public int _damage = 10;

        private bool _canDestroyOnGround = false;

        private Rigidbody2D _rb;
        private SpriteRenderer _sprite;

        public int attackPower => _damage;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();

            _rb.gravityScale = 0;

        }
        private void Start()
        {

            StartCoroutine(FallProcess());
        }


        IEnumerator FallProcess()
        {
            float _timer = 0f;
            while (_timer < _warningDuration)
            {
                _timer += Time.deltaTime;

                yield return null;
            }
            _rb.gravityScale = _fallGravity;

            yield return new WaitForSeconds(0.4f);

            _canDestroyOnGround = true;
        }

        private void OnTriggerEnter2D(Collider2D _collision)
        {
            if (_collision.CompareTag("Ground"))
            {
                
                if (_canDestroyOnGround)
                {
                    DestroyDebris();
                }
            }
            else if (_collision.CompareTag("Player"))
            {
                // 플레이어 피격 처리
                IDamageable target = _collision.GetComponent<IDamageable>();
                if (target != null)
                {
                    // IAttacker 정보를 넘겨주며 데미지 입힘
                    target.TakeDamage(attackPower, this);
                }

                DestroyDebris();
            }
        }

        private void DestroyDebris()
        {
           
            Destroy(gameObject);
        }

      
        public Vector3 GetAttackerPos()
        {
            return transform.position;
        }

        // IAttacker 인터페이스 구현: 공격 함수 (Trigger로 처리하므로 비워둠)
        public void Attack(IDamageable target)
        { 
        }
    }

}


