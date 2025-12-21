using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Common;

namespace Scripts.Skill
{
    using Scripts.Player;
    public abstract class Buff
    {
        protected BuffState _data;
        protected Player _caster;

        public Buff(BuffState data, Player caster)
        {
            _data = data;
            _caster = caster;
        }

        public virtual void OnActive()
        {

        }

        public virtual void OnUpdate()
        {
            if (_data._isCounted == true)
            {
                return;
            }    

            //시간 기반 스킬이라면
            if (_data._lastActive + _data._elapsedTime <= Time.time)
            {
                _data._IsExpired = true;
            }
            return;
        }

        public virtual void OnUnActive()
        {

        }

        public void DecreaseCount()
        {
            --_data._counting;
            if (_data._counting == 0)
            {
                _data._IsExpired = true;
            }
            return;
        }

        public ulong GetId()
        {
            return _data._id;
        }

        public bool IsEqualToId(ulong id)
        {
            return _data._id == id;
        }

        public bool IsCountBaseSkill()
        {
            return _data._isCounted;
        }
        public bool IsInitial()
        {
            return _data._IsInitial;
        }

        public void SetIsNotInit()
        {
            _data._IsInitial = false;
        }

        public bool IsExpired()
        {
            return _data._IsExpired;
        }
    }
}

