using Scipts.Skill;
using Scripts.Common;
using Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Skill
{
    public class ImmotalPlayer : Buff
    {
        public ImmotalPlayer(BuffState data, Player_Temp caster) 
            : base(data, caster)
        {
        }

        public override void OnActive()
        {
            base.OnActive();
            _caster._skillAction |= (ulong)eSkillBitMask.Immotal;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnUnActive()
        {
            base.OnUnActive();
            _caster._skillAction &= ~(ulong)eSkillBitMask.Immotal;
        }


    }

}
