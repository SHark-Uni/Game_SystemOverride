using Scipts.Boss;
using Scripts.Boss;
using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scipts.Boss
{
    public class BossFloorAttackState : BossSuperState
    {

        private float _timer;
        private bool _isAttackDone;

        public BossFloorAttackState(Boss_Temp _boss, BossStateMachine<Boss_Temp> _stateMachine, Rigidbody2D _bossrb, Animator _bossam) : base(_boss, _stateMachine, "IsFloor", _bossrb, _bossam)
        {

        }

        public override void Enter()
        {
            base.Enter();
            _timer = 0f;
            _isAttackDone = false;

            _bossOwner.GetComponent<SpriteRenderer>().color = Color.red;

        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
            _timer += Time.deltaTime;

            //  0.5초 후에 공격 발동 
            if (_timer > _bossOwner._floorAttackDelay && !_isAttackDone)
            {
                _isAttackDone = true;
                Attack();
            }

            //  2.0초 뒤에 패턴 종료
            if (_timer > _bossOwner._floorStateDuration)
            {
                _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _bossOwner.GetComponent<SpriteRenderer>().color = Color.white;
        }

        private void Attack()
        {
            // 장판 프리팹 생성
            if (_bossOwner._floorAttackPrefab != null)
            {
                GameObject floor = GameObject.Instantiate(
                    _bossOwner._floorAttackPrefab,
                    _bossOwner._floorAttackSpawnPoint.position,
                    Quaternion.identity
                );

                // 정해놓은 시간 뒤에 알아서 장판이 사라지게 설정
                GameObject.Destroy(floor, _bossOwner._floorPrefabLifeTime);
            }

        }


    }
}
