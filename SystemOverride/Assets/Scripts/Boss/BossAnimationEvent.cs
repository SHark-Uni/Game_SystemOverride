using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Boss
{
    public class BossAnimationEvent : MonoBehaviour
    {
        Boss_Temp _boss;

        void Start()
        {
            _boss = GetComponentInParent<Boss_Temp>();
        }

        public void OnAttackEnd()
        {
            _boss.BossSetAnimTrigger();
        }
    }
}