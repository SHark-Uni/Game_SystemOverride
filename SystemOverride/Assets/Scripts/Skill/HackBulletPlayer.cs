using Scripts.Common;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Skill
{
    using Scripts.Player;
    public class HackBulletPlayer : Buff
    {
        public HackBulletPlayer(BuffState data, Player caster) 
            : base(data, caster)
        {
        }

        public override void OnActive()
        {
            base.OnActive();
            _caster._skillAction |= (ulong)eSkillBitMask.HackBullet;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnUnActive()
        {
            base.OnUnActive();
            _caster._skillAction &= ~(ulong)eSkillBitMask.HackBullet;
        }

    }
}

