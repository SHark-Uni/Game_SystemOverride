using Scipts.Skill;
using Scripts.Common;
using Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Skill
{
    public class InvisiblePlayer : Buff
    {
        public InvisiblePlayer(BuffState data, Player_Temp caster) 
            : base(data, caster)
        {

        }

        public override void OnActive()
        {
            base.OnActive();
            _caster._skillAction = _caster._skillAction | (ulong)eSkillBitMask.Invisible;
            _caster.gameObject.layer = (int)eLayerNumber.Ghost;
            _caster.ChangeInvisibleMaterial();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnUnActive()
        {
            base.OnUnActive();
            _caster._skillAction = _caster._skillAction & ~((ulong)eSkillBitMask.Invisible);
            _caster.gameObject.layer = (int)eLayerNumber.Player;
            _caster.ResetMaterial();
        }


    }
}

