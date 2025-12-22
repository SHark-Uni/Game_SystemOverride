using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scipts.Boss
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
            _warningLine.SetActive(true);
            _warningLine.GetComponent<SpriteRenderer>().color = Color.yellow; // 노란색 경고
            _beamHitBox.SetActive(false);
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