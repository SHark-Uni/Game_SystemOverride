using Scripts.Common;
using Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Skill
{
    using Scripts.Player;
    public class LaserBlasterPlayer : Buff
    {
        public LaserBlasterPlayer(BuffState data, Player caster) 
            : base(data, caster)
        {
        }
        public override void OnActive()
        {
            base.OnActive();
            _caster._skillAction = _caster._skillAction | (ulong)eSkillBitMask.LaserBuster;
            SoundManager.instance.PlaySFX("BlasterMode", _caster.playerPosition);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnUnActive()
        {
            base.OnUnActive();
            _caster._skillAction = _caster._skillAction & ~((ulong)eSkillBitMask.LaserBuster);
        }
    }

   

}

