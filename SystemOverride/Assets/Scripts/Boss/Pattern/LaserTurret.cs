using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scripts.Boss
{
    public class LaserTurret : MonoBehaviour
    {
        public GameObject _warningLine;
        public GameObject _beamHitBox;

        private Transform _target;
        public float _trunSpeed;

        public void Init(Transform target, float _speed)
        {
            _target = target;
            _trunSpeed = _speed;
            if (_warningLine != null)
            {
                _warningLine.SetActive(true);
                _warningLine.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            if (_beamHitBox != null) _beamHitBox.SetActive(false);
        }

        public void StartAttackSequence(float trackTime, float lockTime, float fireTime)
        {
            StartCoroutine(AttackRoutine(trackTime, lockTime, fireTime));
        }

        IEnumerator AttackRoutine(float trackTime, float lockTime, float fireTime)
        {
            // 1. 조준 단계 (Track)
            float timer = 0f;
            while (timer < trackTime)
            {
                timer += Time.deltaTime;
                TrackTarget(); // 계속 플레이어를 따라옴
                yield return null;
            }

            // 2. 고정 및 경고 단계 (LockOn)
            LockOn();
            yield return new WaitForSeconds(lockTime);

            // 3. 발사 단계 (Fire)
            Fire();
            // (선택사항) 발사 시 화면 흔들림 효과 등 추가 가능
            yield return new WaitForSeconds(fireTime);

            // 4. 종료 (Destroy)
            Destroy(gameObject);
        }

        public void TrackTarget()
        {
            if (_target == null) return;
            // 방향 백터
            Vector3 dir = _target.position - transform.position;
            // 각도값 구하고 angle에 저장
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // 목표 각도 계산
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // _turnSpeed 속도로 회전해서 목표물 조준
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _trunSpeed * Time.deltaTime);
        }

        // 경고 표시
        public void LockOn()
        {
            if (_warningLine != null)
                _warningLine.GetComponent<SpriteRenderer>().color = Color.red;
        }

        public void Fire()
        {
            if (_warningLine != null) _warningLine.SetActive(false);
            if (_beamHitBox != null) _beamHitBox.SetActive(true);
        }
    }

}