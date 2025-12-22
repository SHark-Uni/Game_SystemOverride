using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Scripts.Boss
{
    public class FallDebris : MonoBehaviour
    {
        [Header("낙하 설정")]
        public float _warningDuration = 0.3f; // 떨어지기 전 대기 시간 
        public float _fallGravity = 0.5f;     // 떨어질 때 중력 (속도)
        public int _damage = 10;

        private Rigidbody2D _rb;
        private SpriteRenderer _sprite;

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
        }

        private void OnTriggerEnter2D(Collider2D _collision)
        {
            if (_collision.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }

            else if (_collision.CompareTag("Player"))
            {
                // 데미지주는 스크립트
                Destroy(gameObject);
            }
        }


    }

}