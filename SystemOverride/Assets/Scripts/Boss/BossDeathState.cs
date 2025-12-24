using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Boss
{
    public class BossDeathState : BossOnGroundState
    {
        public BossDeathState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
                    : base(owner, stateMachine, name, rb, am)
        {
            name = "Death";
        }

        void Death()
        {
            if(_bossOwner._bossCurrentHp <= 0)
            {
                Debug.Log("보스 사망");
                SoundManager.instance.PlaySFX("BossDead", _bossOwner.transform.position);
                // 사망 후 오브젝트 삭제
                Object.Destroy(_bossOwner.gameObject, 2f);
            }
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            Death();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
