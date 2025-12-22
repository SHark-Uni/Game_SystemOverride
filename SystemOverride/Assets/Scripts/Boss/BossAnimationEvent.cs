using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Boss
{
    public class BossAnimationEvent : MonoBehaviour
    {
        Boss_Temp _bossTemp;

        void Start()
        {
            _bossTemp = GetComponentInParent<Boss_Temp>();
        }

        public void OnAttackEnd()
        {
            _bossTemp.BossSetAnimTrigger();
        }

        public void OnSoundEnd()
        {
            _bossTemp.BossSetAnimTrigger();
        }
    }
}