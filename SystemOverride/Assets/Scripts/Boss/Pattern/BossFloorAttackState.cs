using Scripts.Boss;
using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common;

namespace Scripts.Boss
{
    public class BossFloorAttackState : BossSuperState
    {
        private bool _hasLanded; // 땅에 착지했는지 체크
        private float _waitTimer;
        private float _ignoreGroundCheckTimer; // 바닥감지 끄기 위한 변수설정 (패턴 여러번 쓰기 위해)
        // 패턴 횟수
        private int _currentSlamCount;
        private const int _maxSlamCount = 3;

        // 낙하 관련 설정
        private float _teleportHeight = 3.0f;
        private float _slamSpeed = -20.0f;

        public BossFloorAttackState(Boss_Temp boss, BossStateMachine<Boss_Temp> stateMachine, string animBoolName, Rigidbody2D bossrb, Animator bossam) : base(boss, stateMachine, animBoolName, bossrb, bossam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _currentSlamCount = 0;
            StartSlam();
        }

        private void StartSlam()
        {
            _hasLanded = false;
            _waitTimer = 0f;

            // 해당 시간동안 바닥체크 무시
            _ignoreGroundCheckTimer = 0.05f;

            _bossOwner.BossSetVelocity(0, 0);
            _bossOwner.BossAnim.SetTrigger("Floor");

            Vector3 _targetPos;
            // 위치 이동
            if (_bossOwner.BossCenterPos != null)
            {
                _targetPos = _bossOwner.BossCenterPos.position;
               

            }
            else
            {
                _targetPos = _bossOwner.transform.position;
            }
            _targetPos.y += _teleportHeight;
            _bossOwner.transform.position = _targetPos;
            // 낙하 시작
            _bossOwner.BossSetVelocity(0, _slamSpeed);
        }

        public override void EntityUpdate()
        {
            if (!_hasLanded)
            {
                // 타이머 감소
                if (_ignoreGroundCheckTimer > 0)
                {
                    _ignoreGroundCheckTimer -= Time.deltaTime;
                }

                // 타이머가 끝났을 때만 바닥 체크
                if (_ignoreGroundCheckTimer <= 0)
                {
                    _bossOwner.CheckOnGround();

                    // 착지 체크
                    bool landedCondition = _bossOwner.IsGrounded && _bossOwner.BossRb.velocity.y <= 0.1f;

                    if (landedCondition)
                    {
                        PerformImpact();
                    }
                }
            }
            // 착지 했다면 해당 코드의 내용 실행
            else
            {
                _bossOwner.BossSetVelocity(0, 0);
                _waitTimer += Time.deltaTime;

                if (_waitTimer >= _bossOwner._floorStateDuration)
                {
                    _currentSlamCount++;
                    // 정해진 횟수만큼 패턴을 진행하지 않았으면 다시 실행
                    if (_currentSlamCount < _maxSlamCount)
                    {
                        StartSlam();
                    }
                    // 패턴이 최대 횟수만큼 진행했으면 Idle 상태로 전환
                    else
                    {
                        _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
                    }
                }
            }
        }


        public override void Exit()
        {
            base.Exit();
            _bossOwner.GetComponent<SpriteRenderer>().color = Color.white;
        }

        private void PerformImpact()
        {

            _hasLanded = true;

            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySFX("BossFloorAttack", _bossOwner.transform.position);
            }
            SpawnShockwave(-1);
            SpawnShockwave(1);
        }

        private void SpawnShockwave(int direction)
        {
            if (_bossOwner._floorAttackPrefab == null) return;
            // 보스 발밑 위치 계산
            Vector3 spawnPos = _bossOwner.transform.position;
            // 바닥보다 살짝 위로 스폰 ( 아니면 바로 사라짐 )
            spawnPos.y += 1.7f;

            GameObject shockwave = Object.Instantiate(_bossOwner._floorAttackPrefab, spawnPos, Quaternion.identity);

            Vector3 scale = shockwave.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            shockwave.transform.localScale = scale;

            Rigidbody2D waveRb = shockwave.GetComponent<Rigidbody2D>();
            if (waveRb != null)
            {
                float waveSpeed = 12f;
                waveRb.velocity = new Vector2(direction * waveSpeed, 0);
            }
        }
    }
}