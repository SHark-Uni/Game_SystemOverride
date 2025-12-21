using Scipts.Boss;
using Scripts.Boss;
using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Boss
{
    public class BossWalkState : BossOnGroundState
    {
        public BossWalkState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
                : base(owner, stateMachine, name, rb, am)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();
            Vector2 _playerpos = _bossOwner._playerPos.position;
            Vector2 _bosspos = _bossOwner.transform.position;
            // 보스와 플레이어 사이의 거리 계산
            float dist = Vector2.Distance(_playerpos, _bosspos);

            if (dist > 2) // 플레이어와 보스의 거리가 2 이상일 때만 이동
            {
                //Debug.Log("보스 이동 상태");
                float dir = _playerpos.x - _bosspos.x;

                Vector2 direction = (_playerpos - _bosspos).normalized;
                Vector3 Pos3D = direction;

                _bossAm.SetBool("Move", true);

                _bossOwner.transform.Translate(Pos3D * 1f * Time.deltaTime);
            }
            else if (dist <= 2f)
            {
                //Debug.Log("보스 이동 상태, 2 이하");
                _bossAm.SetBool("Move", false);

                return;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
