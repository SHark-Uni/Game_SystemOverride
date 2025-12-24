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
        private float _trackDuration =2.5f; // 따라다니는 시간
        private float _lockDuration = 0.5f;  // 멈춰서 경고하는 시간
        private float _fireDuration = 1.0f;  // 빔 쏘는 시간

        private float _hideDelay = 1.0f;
        private float _appearDelay = 1.0f;




        // 보스 컴포넌트들을 껐다 켰다 하기 위해 저장
        private SpriteRenderer _bossRenderer;
        private Collider2D _bossCollider;

        private Coroutine _attackCoroutine;
        public BossLazerAttackState(Boss_Temp _boss, BossStateMachine<Boss_Temp> _stateMachine, Rigidbody2D _bossrb, Animator _bossam) : base(_boss, _stateMachine, "IsHide", _bossrb, _bossam)
        {
        }


        public override void Enter()
        {
            base.Enter();
  

            //  보스 숨기기
            _bossRenderer = _bossOwner.GetComponent<SpriteRenderer>();
            _bossCollider = _bossOwner.GetComponent<Collider2D>();

            _attackCoroutine = _bossOwner.StartCoroutine(AttackSequence());
            SoundManager.instance.PlaySFX("BossLazerCharging", _bossOwner.transform.position);
        }

        public override void EntityUpdate()
        { 
        }
        public override void Exit()
        {
            if (_attackCoroutine != null)
                _bossOwner.StopCoroutine(_attackCoroutine);

            base.Exit();

            // 상태가 끝날 때 보스 다시 등장 및 복구
            if (_bossRenderer) _bossRenderer.enabled = true;
            if (_bossCollider) _bossCollider.enabled = true;


            
        }

        private IEnumerator AttackSequence()
        {
            // 숨기 애니메이션 시작
            _bossOwner.BossAnim.SetTrigger("IsHide");

            // 애니메이션이 재생되는 동안 대기
            yield return new WaitForSeconds(_hideDelay);

            // 보스 사라짐 (투명 + 무적)
            if (_bossRenderer) _bossRenderer.enabled = false;
            if (_bossCollider) _bossCollider.enabled = false;

            // 터렛 소환 및 공격 시작
            SpawnTurretAndFire();
            yield return new WaitForSeconds(_trackDuration + _lockDuration);

            if (SoundManager.instance != null)
            {
               
                SoundManager.instance.PlaySFX("BossLazerFire", _bossOwner.transform.position);
            }


            if (_bossRenderer) _bossRenderer.enabled = true;
            if (_bossCollider) _bossCollider.enabled = true;


            
            _bossOwner.BossAnim.SetTrigger("Appear");

            // 애니메이션이 끝날 때까지 대기
            yield return new WaitForSeconds(_appearDelay);

            
            _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
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
