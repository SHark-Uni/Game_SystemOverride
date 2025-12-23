using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Boss;

namespace Scipts.Boss
{
    public class MapHazardSpawner : MonoBehaviour
    {
        [Header("생성 설정")]
        public GameObject[] _debrisPrefabs; // 떨어질 프리펩들 리스트로 관리
        public Transform _spawnAreaMin; // 맨 왼쪽 위 끝
        public Transform _spawnAreaMax; // 맵 오른쪽 위 끝

        [Header("타이밍 설정")]
        public float _spawnIntervalMin = 1f; // 최소 생성 간격
        public float _spawnIntervalMax = 3f; // 최대 생성 간격

        [Header("물량 설정")] // 한 웨이브당 생성 개수
        public int _minSpawnCount = 2;
        public int _maxSpawnCount = 4;
        public float _spawnStagger = 0.2f; // 개별적으로 낙하 시간차 두기


        private bool _isSpawning = false;
        public void StartSpawning()
        {
            if (!_isSpawning)
            {
                _isSpawning = true;
                StartCoroutine(SpawnRoutine());
            }
        }

        public void StopSpawning()
        {
            _isSpawning = false;
            StopAllCoroutines();
        }

        IEnumerator SpawnRoutine()
        {
            while (_isSpawning)
            {
                // 랜덤 시간 대기 ( 생성 주기 조절)
                float _waitTime = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
                yield return new WaitForSeconds(_waitTime);

                int _count = Random.Range(_minSpawnCount, _maxSpawnCount + 1);

                for (int i = 0; i < _count; i++)
                {
                    SpawnDebris();
                    yield return new WaitForSeconds(Random.Range(0.05f, _spawnStagger));
                }

            }
        }

        private void SpawnDebris()
        {
            if (_debrisPrefabs.Length == 0) return;

            float _randomX = Random.Range(_spawnAreaMin.position.x, _spawnAreaMax.position.x);
            Vector3 _spawnPos = new Vector3(_randomX, _spawnAreaMin.position.y, 0);

            int _randomIndex = Random.Range(0, _debrisPrefabs.Length);
            GameObject _prefab = _debrisPrefabs[_randomIndex];

            Instantiate(_prefab, _spawnPos, Quaternion.identity);
        }


    }

}