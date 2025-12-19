using Scipts.Boss;
using Scripts.BossStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scripts.Boss
{
    public class BossAttackState : BossOnGroundState
    {
        AnimatorStateInfo _bossainInfo;
        bool _bossAttackFinished = false;

        public BossAttackState(Boss_Temp owner, BossStateMachine<Boss_Temp> stateMachine, string name, Rigidbody2D rb, Animator am)
                : base(owner, stateMachine, name, rb, am)
        {
            _bossainInfo = _bossAm.GetCurrentAnimatorStateInfo(0);
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("BossAttackState Enter");
            _bossAm.SetBool("Attack", true);
        }

        public override void EntityUpdate()
        {
            base.EntityUpdate();

            // 애니메이션 이름 체크
            if (_bossainInfo.IsName("Attack"))
            {
                // normalizedTime은 0.0 ~ 1.0 (1.0 이상이면 애니메이션이 끝난 것)
                if (_bossainInfo.normalizedTime >= 1.0f)
                {
                    Debug.Log(_bossainInfo.normalizedTime);
                    // 애니메이션이 끝나면 대기 상태로 전환
                    _bossAm.SetBool("Attack", false);
                    _bossStateMachine.ChangeState(_bossOwner.bossIdleState);
                    _bossAttackFinished = true;
                }
                // normalizedTime이 1.0f 초과일 때는 리턴
                else if (_bossainInfo.normalizedTime > 1.0f)
                {
                    return;
                }
            }
        }

        public override void Exit()
        {
            _bossAm.SetBool("Attack", false);
            base.Exit();
        }
    }
}
