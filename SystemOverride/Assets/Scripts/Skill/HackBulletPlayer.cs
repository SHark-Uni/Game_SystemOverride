using Scipts.Skill;
using Scripts.Common;
using Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Skill
{
    public class HackBulletPlayer : Buff
    {
        public HackBulletPlayer(BuffState data, Player_Temp caster) 
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

