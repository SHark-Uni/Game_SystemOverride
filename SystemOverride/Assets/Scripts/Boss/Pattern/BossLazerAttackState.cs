using Scripts.Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.BossStateMachine;
using scripts.Boss;


namespace Scripts.Boss
{
    public class BossLazerAttackState : BossSuperState
    {
        private float _trackDuration = 1.5f; // 따라다니는 시간
        private float _lockDuration = 0.5f;  // 멈춰서 경고하는 시간
        private float _fireDuration = 1.0f;  // 빔 쏘는 시간

        // 전체 패턴 시간 (= 위 시간들의 합 + 여유시간)
        private float _totalDuration;
        private float _timer;

        // 보스 컴포넌트들을 껐다 켰다 하기 위해 저장
        private SpriteRenderer _bossRenderer;
        private Collider2D _bossCollider;
        public BossLazerAttackState(Boss_Temp _boss, BossStateMachine<Boss_Temp> _stateMachine, Rigidbody2D _bossrb, Animator _bossam) : base(_boss, _stateMachine, "IsLazer", _bossrb, _bossam)
        {
            _totalDuration = _trackDuration + _lockDuration + _fireDuration;
        }


        public override void Enter()
        {
            base.Enter();
            _timer = 0f;

            //  보스 숨기기
            _bossRenderer = _bossOwner.GetComponent<SpriteRenderer>();
            _bossCollider = _bossOwner.GetComponent<Collider2D>();

            if (_bossRenderer) _bossRenderer.enabled = false;
            if (_bossCollider) _bossCollider.enabled = false;



            // 터렛 소환 및 실행
            SpawnTurretAndFire();
        }

        public override void EntityUpdate()
        {
            _timer += Time.deltaTime;

            if (_timer >= _totalDuration)
            {
                // 패턴 종료 -> Idle로 이동
                _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
            }

        }
        public override void Exit()
        {
            base.Exit();

            // 보스 다시 등장
            if (_bossRenderer) _bossRenderer.enabled = true;
            if (_bossCollider) _bossCollider.enabled = true;


            // _bossOwner.BossAnim.SetTrigger("Appear");
        }

        private void SpawnTurretAndFire()
        {
            // 예외 처리: 프리팹이나 스폰 포인트가 없으면 중단
            if (_bossOwner._turretPrefab == null || _bossOwner._ceilingSpawnPoints == null) return;

            // 플레이어 찾기
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            // [수정된 부분] 랜덤 선택이 아니라 반복문을 통해 "모든" 위치에 생성
            for (int i = 0; i < _bossOwner._ceilingSpawnPoints.Length; i++)
            {
                Transform spawnPoint = _bossOwner._ceilingSpawnPoints[i];

                // 혹시 배열 안에 빈 칸이 있을 수 있으니 체크
                if (spawnPoint == null) continue;

                // 터렛 생성
                GameObject turretObj = Object.Instantiate(_bossOwner._turretPrefab, spawnPoint.position, Quaternion.identity);
                LaserTurret turretScript = turretObj.GetComponent<LaserTurret>();

                if (turretScript != null)
                {
                    // 모든 터렛이 동시에 플레이어를 조준하고 발사 시퀀스 시작
                    turretScript.Init(player.transform, 180f);
                    turretScript.StartAttackSequence(_trackDuration, _lockDuration, _fireDuration);
                }
            }
        }
    }
}
